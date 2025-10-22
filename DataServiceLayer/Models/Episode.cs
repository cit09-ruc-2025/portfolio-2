using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class Episode
{
    public string EpisodeMediaId { get; set; } = null!;

    public string SeriesMediaId { get; set; } = null!;

    public int? EpisodeNumber { get; set; }

    public int? SeasonNumber { get; set; }

    public Media EpisodeMedia { get; set; } = null!;

    public Media SeriesMedia { get; set; } = null!;
}
