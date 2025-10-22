using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class FavoritePerson
{
    public Guid UserId { get; set; }

    public string PeopleId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Person People { get; set; } = null!;

    public User User { get; set; } = null!;
}
