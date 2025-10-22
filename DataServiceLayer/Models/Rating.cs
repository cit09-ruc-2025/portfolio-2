using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Rating
{
    public Guid UserId { get; set; }

    public string MediaId { get; set; } = null!;

    public int? Rating1 { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Media Media { get; set; } = null!;

    public Review? Review { get; set; }

    public User User { get; set; } = null!;
}
