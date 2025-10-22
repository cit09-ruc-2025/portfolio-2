using System;
using System.Collections.Generic;

namespace DataServiceLayer.Models;

public partial class MediaLanguage
{
    public string MediaId { get; set; } = null!;

    public string LanguageName { get; set; } = null!;

    public Media Media { get; set; } = null!;
}
