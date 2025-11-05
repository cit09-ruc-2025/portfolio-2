using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public bool? EmailVerified { get; set; }

    public string? ProfileUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public ICollection<FavoriteMedia> FavoriteMedia { get; set; } = new List<FavoriteMedia>();

    public ICollection<FavoritePerson> FavoritePeople { get; set; } = new List<FavoritePerson>();

    public ICollection<UserList> Lists { get; set; } = new List<UserList>();

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public ICollection<RecentlyViewed> RecentlyVieweds { get; set; } = new List<RecentlyViewed>();

    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    public ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();

    public ICollection<WatchHistory> WatchHistories { get; set; } = new List<WatchHistory>();
}
