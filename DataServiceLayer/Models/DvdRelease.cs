using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class DvdRelease
{
    public string MediaId { get; set; } = null!;

    public DateOnly? ReleaseDate { get; set; }

    public Media Media { get; set; } = null!;
}
