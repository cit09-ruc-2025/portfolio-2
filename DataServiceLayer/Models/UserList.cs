using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class UserList
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public User? User { get; set; }

    public ICollection<Media> Media { get; set; } = new List<Media>();

    public ICollection<Person> People { get; set; } = new List<Person>();
}
