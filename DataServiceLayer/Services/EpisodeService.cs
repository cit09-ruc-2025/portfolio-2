using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceLayer.Dtos;
using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Services
{
  public class EpisodeService : IEpisodeService
  {
    private readonly string? _connectionString;

    public EpisodeService(string? connectionString)
    {
      _connectionString = connectionString;
    }

    public List<EpisodeList> GetEpisodeList(string mediaId)
    {
      var db = new MediaDbContext(_connectionString);

      var query = db.Episodes
          .Include(e => e.EpisodeMedia)
          .Where(x => x.SeriesMediaId == mediaId);

      var episodeList = query.GroupBy(e => e.SeasonNumber)
                              .Select(g => new EpisodeList
                              {
                                Season = g.Key,
                                Episodes = g.Select(e => new EpisodeDTO
                                {
                                  Title = e.EpisodeMedia.Titles
                                            .OrderBy(t => t.Ordering)
                                            .Select(t => t.Title1).FirstOrDefault(),
                                  Id = e.EpisodeMedia.Id,
                                  Plot = e.EpisodeMedia.Plot,
                                  EpisodeNumber = e.EpisodeNumber

                                })
                                .OrderBy(e => e.EpisodeNumber)
                                .ToList()

                              }).ToList();

      if (episodeList == null) return null;

      return episodeList;
    }
  }
}