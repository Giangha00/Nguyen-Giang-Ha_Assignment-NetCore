namespace MusicManagement.Models;

public class SongViewModel
{
    public List<Song> Songs { get; set; }
    
    public int? SingerId { get; set; }
    
    public int? ComposerId { get; set; }
}