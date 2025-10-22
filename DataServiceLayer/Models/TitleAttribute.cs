using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class TitleAttribute
{
    public string? Comment { get; set; }

    public Guid TitleId { get; set; }

    public Title Title { get; set; } = null!;
}
