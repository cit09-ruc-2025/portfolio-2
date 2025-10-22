using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class RecentlyViewed
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? MediaId { get; set; }

    public string? PeopleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Media? Media { get; set; }

    public Person? People { get; set; }

    public User? User { get; set; }
}
