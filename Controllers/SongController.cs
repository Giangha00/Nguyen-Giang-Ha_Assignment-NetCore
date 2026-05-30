    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using MusicManagement.Data;
    using MusicManagement.Helpers;
    using MusicManagement.Models;

    namespace MusicManagement.Controllers;

    public class SongController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly ILogger<SongController> _logger;

        public SongController(AppDbContext context, IPhotoService photoService, ILogger<SongController> logger)
        {
            _context = context;
            _photoService = photoService;
            _logger = logger;
        }

        // GET
        public async Task<IActionResult> Index(SongViewModel filter, int? pageIndex)
        {
            var query = _context.Songs
                .Include(x => x.Singer)
                .Include(x => x.Composer)
                .AsQueryable();

            // Filter Singer
            if (filter.SingerId.HasValue)
            {
                query = query.Where(x =>
                    x.SingerId == filter.SingerId.Value);
            }

            // Filter Composer
            if (filter.ComposerId.HasValue)
            {
                query = query.Where(x =>
                    x.ComposerId == filter.ComposerId.Value);
            }

            // Search keyword
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(x =>
                    x.Title.Contains(filter.Keyword) ||
                    x.Lyrics.Contains(filter.Keyword));
            }

            // Filter status
            if (filter.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == filter.Status.Value);
            }
            
            // Filter date range
            if (!string.IsNullOrWhiteSpace(filter.DateRange))
            {
                var dates = filter.DateRange.Split(" - ");

                if (dates.Length == 2)
                {
                    bool startOk = DateTime.TryParse(dates[0], out var startDate);

                    bool endOk = DateTime.TryParse(dates[1], out var endDate);

                    if (startOk && endOk)
                    {
                        endDate = endDate.AddDays(1);

                        query = query.Where(x =>
                            x.ReleaseDate >= startDate &&
                            x.ReleaseDate < endDate);
                    }
                }
            }

            // Sorting
            query = filter.SortOrder switch
            {
                "id_desc" => query.OrderByDescending(x => x.Id),

                "name_asc" => query.OrderBy(x => x.Title),

                "name_desc" => query.OrderByDescending(x => x.Title),

                "date_asc" => query.OrderBy(x => x.ReleaseDate),

                "date_desc" => query.OrderByDescending(x => x.ReleaseDate),

                "created_asc" => query.OrderBy(x => x.CreatedAt),

                "created_desc" => query.OrderByDescending(x => x.CreatedAt),

                _ => query.OrderBy(x => x.Id)
            };

            // Load dropdown
            filter.Singers = await _context.Singers.ToListAsync();

            filter.Composers = await _context.Composers.ToListAsync();

            int pageSize = 10;

            filter.Songs = await PaginatedList<Song>.CreateAsync(
                query.AsNoTracking(),
                pageIndex ?? 1,
                pageSize
            );

            return View(filter);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var singers = await _context.Singers.ToListAsync();
            var composer = await _context.Composers.ToListAsync();
            ViewBag.Singers = new SelectList(singers, "Id", "Name");
            ViewBag.Composers = new SelectList(composer, "Id", "Name");
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Song song)
        {
            _logger.LogInformation("Song Created");
            if (!ModelState.IsValid) {
                if (song.ImageFile != null) {
                    _logger.LogInformation("Image Okie");
                    var result = await _photoService.AddPhotoAsync(song.ImageFile);
                    if (result.Error != null) {
                        ModelState.AddModelError("ImageFile", "Tải ảnh thất bại.");
                        return View(song);
                    }
                    _logger.LogInformation("Image Okie1");
                    song.ThumbnailUrl = result.SecureUrl.AbsoluteUri; // Lấy URL từ Cloudinary
                }

                if (song.Mp3File != null)
                {
                    _logger.LogInformation("Mp3 Okie");
                    var result = await _photoService.AddMusicAsync(song.Mp3File);
                    if (result.Error != null) {
                        ModelState.AddModelError("Mp3File", "Tải nhạc thất bại.");
                        return View(song);
                    }
                    _logger.LogInformation("Mp3 Okie1");
                    song.Mp3Link = result.SecureUrl.AbsoluteUri;
                }

                _context.Add(song);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Save Okie");
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            ViewBag.Singers = new SelectList(
                await _context.Singers.ToListAsync(),
                "Id",
                "Name",
                song.SingerId
            );

            ViewBag.Composers = new SelectList(
                await _context.Composers.ToListAsync(),
                "Id",
                "Name",
                song.ComposerId
            );

            return View(song);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Song song)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingSong = await _context.Songs.FindAsync(id);

                if (existingSong == null)
                {
                    return NotFound();
                }

                existingSong.Title = song.Title;
                existingSong.Lyrics = song.Lyrics;
                existingSong.Status = song.Status;
                existingSong.SingerId = song.SingerId;
                existingSong.ComposerId = song.ComposerId;
                existingSong.ReleaseDate = song.ReleaseDate;
                existingSong.UpdatedAt = DateTime.Now;

                // Upload ảnh mới
                if (song.ImageFile != null)
                {
                    var result =
                        await _photoService.AddPhotoAsync(song.ImageFile);

                    if (result.Error != null)
                    {
                        ModelState.AddModelError("", "Upload ảnh thất bại");

                        return View(song);
                    }

                    existingSong.ThumbnailUrl =
                        result.SecureUrl.AbsoluteUri;
                }

                // Upload mp3 mới
                if (song.Mp3File != null)
                {
                    var result =
                        await _photoService.AddMusicAsync(song.Mp3File);

                    if (result.Error != null)
                    {
                        ModelState.AddModelError("", "Upload nhạc thất bại");

                        return View(song);
                    }

                    existingSong.Mp3Link =
                        result.SecureUrl.AbsoluteUri;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Singers = new SelectList(
                await _context.Singers.ToListAsync(),
                "Id",
                "Name",
                song.SingerId
            );

            ViewBag.Composers = new SelectList(
                await _context.Composers.ToListAsync(),
                "Id",
                "Name",
                song.ComposerId
            );

            return View(song);
        }
        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var song = await _context.Songs
                .Include(x => x.Singer)
                .Include(x => x.Composer)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Singer)
                .Include(s => s.Composer)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        // [ValidateAntiForgeryToken] // Nên bổ sung để bảo mật (Cần gửi kèm Token nếu dùng AJAX phức tạp hơn)
        public async Task<IActionResult> DeleteSelected(List<int> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return Json(new { success = false, message = "Không có course nào được chọn." });
            }

            try
            {
                // Lấy danh sách các User có Id nằm trong list gửi lên
                var songsToDelete = await _context.Songs.Where(course => ids.Contains(course.Id)).ToListAsync();
        
                if (songsToDelete.Any())
                {
                    // Xóa cứng (Hard Delete)
                    _context.Songs.RemoveRange(songsToDelete);
            
                    // Hoặc Xóa mềm (nếu bảng có trường Status)
                    /*
                    foreach(var user in usersToDelete) {
                        user.Status = 0;
                    }
                    */
            
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = $"Đã xóa thành công {songsToDelete.Count}." });
                }

                return Json(new { success = false, message = "Không tìm thấy dữ liệu phù hợp." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
    }