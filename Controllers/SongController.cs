using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicManagement.Data;
using MusicManagement.Models;

namespace MusicManagement.Controllers;

public class SongController : Controller
{
    private readonly AppDbContext _context;
    private readonly IPhotoService _photoService;

    public SongController(AppDbContext context, IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var songs = await _context.Songs.ToListAsync();

        return View(songs);
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Song songs) {
        if (ModelState.IsValid) {
            if (songs.ImageFile != null) {
                var result = await _photoService.AddPhotoAsync(songs.ImageFile);
                var mp3_result = await _photoService.AddPhotoAsync(songs.Mp3File);
                if (result.Error != null) {
                    ModelState.AddModelError("ImageFile", "Tải ảnh thất bại.");
                    return View(songs);
                }
                
                songs.ThumbnailUrl = result.SecureUrl.AbsoluteUri; // Lấy URL từ Cloudinary
                
                if (mp3_result.Error != null) {
                    ModelState.AddModelError("Mp3File", "Tải nhạc thất bại.");
                    return View(songs);
                }
                songs.Mp3Link = mp3_result.SecureUrl.AbsoluteUri;
            }

            _context.Add(songs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(songs);
    }
}