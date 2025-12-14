using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;

namespace DataServiceLayer.Interfaces
{
    public interface IEpisodeService
    {
        public List<EpisodeList> GetEpisodeList(string mediaId);
    }
}