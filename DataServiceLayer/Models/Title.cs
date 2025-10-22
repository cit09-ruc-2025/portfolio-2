using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Title
{
    public Guid Id { get; set; }

    public string? MediaId { get; set; }

    public string Title1 { get; set; } = null!;

    public int? Ordering { get; set; }

    public string? Region { get; set; }

    public string? Language { get; set; }

    public Media? Media { get; set; }

    public TitleAttribute? TitleAttribute { get; set; }
}
