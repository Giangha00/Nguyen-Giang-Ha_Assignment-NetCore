using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicManagement.Models;

public class Singer
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Tên không được để trống")]
    public string Name { get; set; } = string.Empty;
    
    public string Biography  { get; set; } = string.Empty;
    
    public string ImageUrl  { get; set; } = string.Empty;
    public virtual ICollection<Song> Courses { get; set; } = new Collection<Song>();
}