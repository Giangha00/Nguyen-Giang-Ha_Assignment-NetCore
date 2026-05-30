using MusicManagement.Helpers;
using MusicManagement.Models;

namespace MusicManagement.Models;

public class SongViewModel
{
    public PaginatedList<Song> Songs { get; set; }

    public List<Singer> Singers { get; set; }

    public List<Composer> Composers { get; set; }

    public int? SingerId { get; set; }

    public int? ComposerId { get; set; }

    public string? Keyword { get; set; }

    public string? DateRange { get; set; }

    public string? SortOrder { get; set; }
    
    public int? Status { get; set; }
}