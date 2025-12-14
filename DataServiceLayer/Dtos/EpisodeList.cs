using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceLayer.Dtos
{
    public class EpisodeList
    {
        public int? Season { get; set; }
        public List<EpisodeDTO> Episodes { get; set; }
    }
}