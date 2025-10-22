using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<MediaPerson> MediaPeople { get; set; } = new List<MediaPerson>();
}
