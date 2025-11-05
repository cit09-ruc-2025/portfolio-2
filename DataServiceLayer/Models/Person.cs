using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Person
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int? BirthDate { get; set; }

    public int? DeathDate { get; set; }

    public string? Description { get; set; }

    public decimal? NameRating { get; set; }

    public ICollection<FavoritePerson> FavoritePeople { get; set; } = new List<FavoritePerson>();

    public ICollection<MediaPerson> MediaPeople { get; set; } = new List<MediaPerson>();

    public ICollection<RecentlyViewed> RecentlyVieweds { get; set; } = new List<RecentlyViewed>();

    public ICollection<UserList> Lists { get; set; } = new List<UserList>();
}
