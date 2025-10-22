using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class FavoriteMedia
{
    public Guid UserId { get; set; }

    public string MediaId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Media Media { get; set; } = null!;

    public User User { get; set; } = null!;
}
