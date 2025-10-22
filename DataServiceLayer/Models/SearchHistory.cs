using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class SearchHistory
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string SearchText { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public User? User { get; set; }
}
