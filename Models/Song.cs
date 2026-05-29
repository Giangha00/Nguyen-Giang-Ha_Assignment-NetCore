using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicManagement.Models;

public class Song
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    public string Title { get; set; } = string.Empty;
    public string Lyrics  { get; set; } = string.Empty;
    public string ThumbnailUrl  { get; set; }
    [NotMapped] // Không tạo cột này trong Database
    public IFormFile? ImageFile { get; set; } 
    public string Mp3Link  { get; set; } = string.Empty;
    [NotMapped] // Không tạo cột này trong Database
    public IFormFile? Mp3File { get; set; } 
    public DateTime ReleaseDate  { get; set; }
    public DateTime CreatedAt  { get; set; }
    public DateTime UpdatedAt  { get; set; }
    public int SingerId  { get; set; }
    [ForeignKey("SingerId")]
    public virtual Singer? Singer { get; set; }
    public int ComposerId  { get; set; }
    [ForeignKey("ComposerId")]
    public virtual Composer? Composer { get; set; }
    public int Status  { get; set; }
}