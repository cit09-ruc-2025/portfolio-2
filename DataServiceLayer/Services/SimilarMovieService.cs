using DataServiceLayer.Interfaces;
using DataServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DataServiceLayer.Services
{
    public class SimilarMovieService : ISimilarMovieService
    {
        private readonly MediaDbContext _db;

        public SimilarMovieService(string? connectionString)
        {
            _db = new MediaDbContext(connectionString);
        }

        public List<Media> GetSimilarMovies(string movieId, int limit = 5)
        {
            var movie = _db.Media
                .Where(m => m.Id == movieId)
                .Select(m => new
                {
                    m.Id,
                    m.Plot,
                    GenreIds = m.Genres.Select(g => g.Id).ToList()  // use Genres instead of MediaGenres
                })
                .FirstOrDefault();

            if (movie == null) return new List<Media>();

            var moviePlotWords = GetPlotWords(movie.Plot);

            // Preload genres for all candidates
            var candidates = _db.Media
                .Where(m => m.Id != movieId)
                .Select(m => new
                {
                    Media = m,
                    GenreIds = m.Genres.Select(g => g.Id).ToList()  // use Genres here too
                })
                .ToList();

            var scoredMovies = candidates.Select(c =>
            {
                int genreScore = movie.GenreIds.Intersect(c.GenreIds).Count();
                int plotScore = GetPlotWords(c.Media.Plot).Intersect(moviePlotWords).Count();
                int totalScore = genreScore + plotScore;
                return new { c.Media, Score = totalScore };
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(limit)
            .Select(x => x.Media)
            .ToList();

            return scoredMovies;
        }

        private HashSet<string> GetPlotWords(string? plot)
        {
            if (string.IsNullOrEmpty(plot)) return new HashSet<string>();

            // text preprocessing: lowercase, remove punctuation, split into words
            var words = Regex.Replace(plot.ToLower(), @"[^\w\s]", "")
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // remove common stopwords
            var stopwords = new HashSet<string> { "the", "a", "an", "and", "of", "in", "on", "at", "to", "with", "for", "is", "are", "was", "were" };
            return words.Where(w => !stopwords.Contains(w)).ToHashSet();
        }
    }
}
