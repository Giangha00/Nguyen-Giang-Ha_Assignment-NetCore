using Microsoft.EntityFrameworkCore;
using MusicManagement.Models;

namespace MusicManagement.Data;

public static class DbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Đảm bảo Database đã được tạo thông qua Migrations
            context.Database.Migrate();

            // 1. Thực hiện Reset dữ liệu
            ClearData(context);

            // 2. Thực hiện Seeding dữ liệu mới
            SeedData(context);
        }
    }

    private static void ClearData(AppDbContext context)
    {
        // Cách 1: Sử dụng EF Core (An toàn cho mọi Database)
        // Lưu ý: Nếu có khóa ngoại (FK), phải xóa theo thứ tự bảng con trước, bảng cha sau.
        if (context.Songs.Any())
        {
            context.Songs.RemoveRange(context.Songs);
            context.SaveChanges();
        }
        
        if (context.Singers.Any())
        {
            context.Singers.RemoveRange(context.Singers);
        }
        
        if (context.Composers.Any())
        {
            context.Composers.RemoveRange(context.Composers);
        }

        // Cách 2: Sử dụng SQL thuần (Tối ưu cho MySQL - Reset cả Identity/Auto Increment)
        // context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
        // context.Database.ExecuteSqlRaw("TRUNCATE TABLE Products;");
        // context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
    }

    private static void SeedData(AppDbContext context)
    {

        var singers = new List<Singer>
        {
            new Singer
            {
                Name = "Sơn Tùng M-TP",
                Biography = "Ca sĩ nổi tiếng Việt Nam",
                ImageUrl = "https://picsum.photos/200?1"
            },
            new Singer
            {
                Name = "Erik",
                Biography = "Ca sĩ nhạc trẻ",
                ImageUrl = "https://picsum.photos/200?2"
            },
            new Singer
            {
                Name = "Hòa Minzy",
                Biography = "Ca sĩ Việt Nam",
                ImageUrl = "https://picsum.photos/200?3"
            },
            new Singer
            {
                Name = "Jack",
                Biography = "Ca sĩ Việt Nam",
                ImageUrl = "https://picsum.photos/200?4"
            },
            new Singer
            {
                Name = "Đức Phúc",
                Biography = "Ca sĩ pop ballad",
                ImageUrl = "https://picsum.photos/200?5"
            }
        };

        context.Singers.AddRange(singers);
        context.SaveChanges();


        var composers = new List<Composer>
        {
            new Composer
            {
                Name = "Khắc Hưng",
                Biography = "Nhạc sĩ nổi tiếng",
                ImageUrl = "https://picsum.photos/200?11"
            },
            new Composer
            {
                Name = "Only C",
                Biography = "Producer Việt Nam",
                ImageUrl = "https://picsum.photos/200?12"
            },
            new Composer
            {
                Name = "Viruss",
                Biography = "Nhạc sĩ và streamer",
                ImageUrl = "https://picsum.photos/200?13"
            },
            new Composer
            {
                Name = "Mr. Siro",
                Biography = "Nhạc sĩ ballad",
                ImageUrl = "https://picsum.photos/200?14"
            },
            new Composer
            {
                Name = "Châu Đăng Khoa",
                Biography = "Nhạc sĩ trẻ",
                ImageUrl = "https://picsum.photos/200?15"
            }
        };

        context.Composers.AddRange(composers);
        context.SaveChanges();


        var songTitles = new List<string>
        {
            "Chúng Ta Của Hiện Tại",
            "Nơi Này Có Anh",
            "Lạc Trôi",
            "Muộn Rồi Mà Sao Còn",
            "Đom Đóm",
            // "Em Gái Mưa",
            // "Sóng Gió",
            // "Bạc Phận",
            // "See Tình",
            // "Ai Chung Tình Được Mãi",
            // "Có Chàng Trai Viết Lên Cây",
            // "Ngày Đầu Tiên",
            // "3107",
            // "Yêu Được Không",
            // "Tháng Tư Là Lời Nói Dối Của Em",
            // "Hẹn Em Ở Lần Yêu Thứ 2",
            // "Có Hẹn Với Thanh Xuân",
            // "Gác Lại Âu Lo",
            // "Khuất Lối",
            // "Waiting For You",
            // "Anh Đã Quen Với Cô Đơn",
            // "Bước Qua Mùa Cô Đơn",
            // "Một Bước Yêu Vạn Dặm Đau",
            // "Sau Tất Cả",
            // "Đi Để Trở Về",
            // "Phía Sau Một Cô Gái",
            // "Hồng Nhan",
            // "Túy Âm",
            // "Người Âm Phủ",
            // "Buồn Của Anh",
            // "Chiều Hôm Ấy",
            // "Tình Yêu Màu Nắng",
            // "Duyên Mình Lỡ",
            // "Em Không Sai Chúng Ta Sai",
            // "Đừng Yêu Nữa Em Mệt Rồi",
            // "Cô Đơn Dành Cho Ai",
            // "Trót Yêu",
            // "Là Bạn Không Thể Yêu",
            // "Chạy Ngay Đi",
            // "Hãy Trao Cho Anh",
            // "Thiên Lý Ơi",
            // "Vì Mẹ Anh Bắt Chia Tay",
            // "Hoa Hải Đường",
            // "Từng Quen",
            // "Ex's Hate Me",
            // "Simple Love",
            // "Bigcityboi",
            // "Anh Nhà Ở Đâu Thế",
            // "Sài Gòn Đau Lòng Quá",
            // "Ngày Mai Người Ta Lấy Chồng"
        };

        var thumbnail = new List<string>
        {
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/cover/f/0/c/6/f0c6b74652e9ed643f3183c7617aaa30.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/covers/3/a/3a9e48bc4df7bbde3acea30cc267f609_1487066528.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w600_r1x1_jpeg/covers/9/8/98e3677733fe52439823d1b1992d9ae0_1483242323.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/cover/d/e/b/0/deb0fa47b10ad47197f213244da2fc48.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w600_r1x1_jpeg/cover/f/7/5/1/f7518e9f9e2b66f2a1670c3b07a3e47f.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/cover/f/0/c/6/f0c6b74652e9ed643f3183c7617aaa30.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/covers/3/a/3a9e48bc4df7bbde3acea30cc267f609_1487066528.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w600_r1x1_jpeg/covers/9/8/98e3677733fe52439823d1b1992d9ae0_1483242323.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w94_r1x1_jpeg/cover/d/e/b/0/deb0fa47b10ad47197f213244da2fc48.jpg",
            "https://photo-resize-zmp3.zmdcdn.me/w600_r1x1_jpeg/cover/f/7/5/1/f7518e9f9e2b66f2a1670c3b07a3e47f.jpg",
            
        };
        
        var mp3Link = new List<string>
        {
            "https://zingmp3.vn/album/Chung-Ta-Cua-Hien-Tai-Single-Son-Tung-M-TP/6BD0W9U7.html",
            "https://zingmp3.vn/album/Noi-Nay-Co-Anh-Single-Son-Tung-M-TP/ZOUEA86A.html",
            "https://zingmp3.vn/album/Lac-Troi-Single-Son-Tung-M-TP/ZOUA7WB9.html",
            "https://zingmp3.vn/album/Muon-Roi-Ma-Sao-Con-Single-Son-Tung-M-TP/6BD0WAFU.html",
            "https://zingmp3.vn/album/Dom-Dom-Single-Jack-J97/670WF0OU.html",
            "https://zingmp3.vn/album/Chung-Ta-Cua-Hien-Tai-Single-Son-Tung-M-TP/6BD0W9U7.html",
            "https://zingmp3.vn/album/Noi-Nay-Co-Anh-Single-Son-Tung-M-TP/ZOUEA86A.html",
            "https://zingmp3.vn/album/Lac-Troi-Single-Son-Tung-M-TP/ZOUA7WB9.html",
            "https://zingmp3.vn/album/Muon-Roi-Ma-Sao-Con-Single-Son-Tung-M-TP/6BD0WAFU.html",
            "https://zingmp3.vn/album/Dom-Dom-Single-Jack-J97/670WF0OU.html",
            
        };

        var random = new Random();

        var songs = new List<Song>();

        for (int i = 1; i <= 50; i++)
        {
            songs.Add(new Song
            {
                Title = $"{songTitles[random.Next(songTitles.Count)]}",

                Lyrics = "Đây là lời bài hát mẫu.",

                ThumbnailUrl =
                    $"{thumbnail[random.Next(songTitles.Count)]}",

                Mp3Link =
                    $"{mp3Link[random.Next(songTitles.Count)]}",

                ReleaseDate =
                    DateTime.Now.AddDays(-random.Next(1, 1000)),

                CreatedAt = DateTime.Now,

                UpdatedAt = DateTime.Now,

                SingerId =
                    singers[random.Next(singers.Count)].Id,

                ComposerId =
                    composers[random.Next(composers.Count)].Id,

                Status = 1
            });
        }

        context.Songs.AddRange(songs);

        context.SaveChanges();
    }
}
