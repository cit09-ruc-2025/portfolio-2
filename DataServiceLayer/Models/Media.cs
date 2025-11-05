using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataServiceLayer.Models;

public partial class Media
{
    public string Id { get; set; } = null!;

    public int? ReleaseYear { get; set; }

    public int? EndYear { get; set; }

    public int? RuntimeMinutes { get; set; }

    public decimal? ImdbAverageRating { get; set; }

    public int? ImdbNumberOfVotes { get; set; }

    public string? Plot { get; set; }

    public string? Awards { get; set; }

    public string? AgeRating { get; set; }

    public string? Poster { get; set; }

    public string? Production { get; set; }

    public string? BoxOffice { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? Metascore { get; set; }

    public decimal? AverageRating { get; set; }

    public DvdRelease? DvdRelease { get; set; }

    public ICollection<Episode> EpisodeEpisodeMedia { get; set; } = new List<Episode>();

    public ICollection<Episode> EpisodeSeriesMedia { get; set; } = new List<Episode>();

    public ICollection<FavoriteMedia> FavoriteMedia { get; set; } = new List<FavoriteMedia>();

    public ICollection<MediaLanguage> MediaLanguages { get; set; } = new List<MediaLanguage>();

    public ICollection<MediaPerson> MediaPeople { get; set; } = new List<MediaPerson>();

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public ICollection<RecentlyViewed> RecentlyVieweds { get; set; } = new List<RecentlyViewed>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<Title> Titles { get; set; } = new List<Title>();

    public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();

    public ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public ICollection<UserList> Lists { get; set; } = new List<UserList>();

    [NotMapped]
    public string? DisplayTitle
    {
        get
        {
            var primaryTitle = Titles.OrderBy(t => t.Ordering).FirstOrDefault();
            if (primaryTitle != null)
            {
                return primaryTitle.Title1;
            }

            return null;
        }
    }
}
