using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Genre
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Media> Media { get; set; } = new List<Media>();
}
