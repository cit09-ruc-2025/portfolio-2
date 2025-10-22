using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class MediaPerson
{
    public string MediaId { get; set; } = null!;

    public string PeopleId { get; set; } = null!;

    public Guid RoleId { get; set; }

    public bool? KnownFor { get; set; }

    public int? Ordering { get; set; }

    public string? JobNote { get; set; }

    public string? Characters { get; set; }

    public Media Media { get; set; } = null!;

    public Person People { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
