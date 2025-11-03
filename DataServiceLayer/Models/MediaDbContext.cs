using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataServiceLayer.Models;

public partial class MediaDbContext : DbContext
{
    private readonly string? _connectionString;

    public MediaDbContext(string? connectionString)
    {
        _connectionString = connectionString;
    }

    // For EF migrations
    public MediaDbContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Logging and sensitive data for debugging
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.EnableSensitiveDataLogging();

        // Use the provided connection string
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _connectionString ?? "Host=localhost;Database=project_portfolio;Username=postgres;Password=neva0518";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    public DbSet<DvdRelease> DvdReleases { get; set; }

    public DbSet<Episode> Episodes { get; set; }

    public DbSet<FavoriteMedia> FavoriteMedia { get; set; }

    public DbSet<FavoritePerson> FavoritePeople { get; set; }

    public DbSet<Genre> Genres { get; set; }

    public DbSet<MediaLanguage> MediaLanguages { get; set; }

    public DbSet<MediaPerson> MediaPeople { get; set; }

    public DbSet<Media> Media { get; set; }

    public DbSet<Person> People { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<RecentlyViewed> RecentlyVieweds { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<SearchHistory> SearchHistories { get; set; }

    public DbSet<Title> Titles { get; set; }

    public DbSet<TitleAttribute> TitleAttributes { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<MediaListItem> MediaListItems { get; set; }
    public DbSet<PeopleListItem> PeopleListItems { get; set; }

    public DbSet<WatchHistory> WatchHistories { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    //     optionsBuilder.EnableSensitiveDataLogging();
    //     optionsBuilder.UseNpgsql(_connectionString);
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("media_type", new[] { "tvShort", "movie", "tvMovie", "short", "tvMiniSeries", "videoGame", "tvEpisode", "video", "tvSpecial", "tvSeries" })
            .HasPostgresEnum("title_type", new[] { "working", "original", "", "imdbDisplayfestival", "workingalternative", "dvd", "imdbDisplay", "tv", "imdbDisplaydvd", "alternative", "festival", "video" })
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<DvdRelease>(entity =>
        {
            entity.HasKey(e => e.MediaId).HasName("dvd_releases_pkey");

            entity.ToTable("dvd_releases");

            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");

            entity.HasOne(d => d.Media).WithOne(p => p.DvdRelease)
                .HasForeignKey<DvdRelease>(d => d.MediaId)
                .HasConstraintName("dvd_releases_media_id_fkey");
        });

        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => new { e.EpisodeMediaId, e.SeriesMediaId }).HasName("episodes_pkey");

            entity.ToTable("episodes");

            entity.Property(e => e.EpisodeMediaId)
                .HasMaxLength(50)
                .HasColumnName("episode_media_id");
            entity.Property(e => e.SeriesMediaId)
                .HasMaxLength(50)
                .HasColumnName("series_media_id");
            entity.Property(e => e.EpisodeNumber).HasColumnName("episode_number");
            entity.Property(e => e.SeasonNumber).HasColumnName("season_number");

            entity.HasOne(d => d.EpisodeMedia).WithMany(p => p.EpisodeEpisodeMedia)
                .HasForeignKey(d => d.EpisodeMediaId)
                .HasConstraintName("episodes_episode_media_id_fkey");

            entity.HasOne(d => d.SeriesMedia).WithMany(p => p.EpisodeSeriesMedia)
                .HasForeignKey(d => d.SeriesMediaId)
                .HasConstraintName("episodes_series_media_id_fkey");
        });

        modelBuilder.Entity<FavoriteMedia>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.MediaId }).HasName("favorite_media_pkey");

            entity.ToTable("favorite_media");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Media).WithMany(p => p.FavoriteMedia)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("favorite_media_media_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteMedia)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("favorite_media_user_id_fkey");
        });

        modelBuilder.Entity<FavoritePerson>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PeopleId }).HasName("favorite_people_pkey");

            entity.ToTable("favorite_people");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PeopleId)
                .HasMaxLength(50)
                .HasColumnName("people_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.People).WithMany(p => p.FavoritePeople)
                .HasForeignKey(d => d.PeopleId)
                .HasConstraintName("favorite_people_people_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FavoritePeople)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("favorite_people_user_id_fkey");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("genres_pkey");

            entity.ToTable("genres");

            entity.HasIndex(e => e.Name, "genres_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("people_pkey");

            entity.ToTable("people");

            entity.HasIndex(e => e.Name, "idx_people_name");

            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.DeathDate).HasColumnName("death_date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.NameRating)
                .HasPrecision(3, 1)
                .HasColumnName("name_rating");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.MediaId }).HasName("ratings_pkey");

            entity.ToTable("ratings");

            entity.HasIndex(e => e.MediaId, "idx_ratings_media_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Rating1).HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Media).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("ratings_media_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("ratings_user_id_fkey");
        });

        modelBuilder.Entity<RecentlyViewed>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("recently_viewed_pkey");

            entity.ToTable("recently_viewed");

            entity.HasIndex(e => e.UserId, "idx_recently_viewed_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.PeopleId)
                .HasMaxLength(50)
                .HasColumnName("people_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Media).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("recently_viewed_media_id_fkey");

            entity.HasOne(d => d.People).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.PeopleId)
                .HasConstraintName("recently_viewed_people_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.RecentlyVieweds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("recently_viewed_user_id_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.MediaId }).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ReviewText)
                .HasMaxLength(5000)
                .HasColumnName("review_text");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Media).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("reviews_media_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("reviews_user_id_fkey");

            entity.HasOne(d => d.Rating).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => new { d.UserId, d.MediaId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_user_id_media_id_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "idx_roles_name");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("search_history_pkey");

            entity.ToTable("search_history");

            entity.HasIndex(e => e.UserId, "idx_search_history_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.SearchText).HasColumnName("search_text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.SearchHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("search_history_user_id_fkey");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("titles_pkey");

            entity.ToTable("titles");

            entity.HasIndex(e => e.MediaId, "idx_titles_media_id");

            entity.HasIndex(e => e.Title1, "idx_titles_title");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Language)
                .HasMaxLength(8)
                .HasColumnName("language");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.Ordering).HasColumnName("ordering");
            entity.Property(e => e.Region)
                .HasMaxLength(8)
                .HasColumnName("region");
            entity.Property(e => e.Title1)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Media).WithMany(p => p.Titles)
                .HasForeignKey(d => d.MediaId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("titles_media_id_fkey");
        });

        modelBuilder.Entity<TitleAttribute>(entity =>
        {
            entity.HasKey(e => e.TitleId).HasName("title_attributes_pkey");

            entity.ToTable("title_attributes");

            entity.Property(e => e.TitleId)
                .ValueGeneratedNever()
                .HasColumnName("title_id");
            entity.Property(e => e.Comment).HasColumnName("comment");

            entity.HasOne(d => d.Title).WithOne(p => p.TitleAttribute)
                .HasForeignKey<TitleAttribute>(d => d.TitleId)
                .HasConstraintName("title_attributes_title_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "idx_users_username");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.EmailVerified)
                .HasDefaultValue(false)
                .HasColumnName("email_verified");
            entity.Property(e => e.HashedPassword).HasColumnName("hashed_password");
            entity.Property(e => e.ProfileUrl).HasColumnName("profile_url");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<WatchHistory>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.MediaId }).HasName("watch_history_pkey");

            entity.ToTable("watch_history");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.MediaId)
                .HasMaxLength(50)
                .HasColumnName("media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            entity.HasOne(d => d.Media).WithMany(p => p.WatchHistories)
                .HasForeignKey(d => d.MediaId)
                .HasConstraintName("watch_history_media_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.WatchHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("watch_history_user_id_fkey");
        });

        modelBuilder.Entity<List>(entity =>
        {
            entity.ToTable("lists");
            entity.HasKey(p => p.Id).HasName("lists_pkey");

            entity.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");

            entity.Property(p => p.Title)
                .HasMaxLength(255)
                .IsRequired()
                .HasColumnName("title");

            entity.Property(p => p.Description)
                .HasMaxLength(500)
                .HasColumnName("description");

            entity.Property(p => p.UserId)
                .IsRequired()
                .HasColumnName("user_id");

            entity.Property(p => p.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            entity.Property(p => p.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            // Relationship: List belongs to a User
            entity.HasOne(p => p.User)
                .WithMany(u => u.Lists)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("lists_user_id_fkey");

            // Media many-to-many
            entity.HasMany(p => p.MediaListItems)
                .WithOne(mli => mli.List)
                .HasForeignKey(mli => mli.ListId)
                .HasConstraintName("media_lists_rel_list_id_fkey");

            // People many-to-many
            entity.HasMany(p => p.PeopleListItems)
                .WithOne(pli => pli.List)
                .HasForeignKey(pli => pli.ListId)
                .HasConstraintName("people_lists_rel_list_id_fkey");
        });

        // MediaListItem 
        modelBuilder.Entity<MediaListItem>(entity =>
        {
            entity.ToTable("media_lists_rel");
            entity.HasKey(ml => new { ml.ListId, ml.MediaId }).HasName("media_lists_rel_pkey");

            entity.Property(ml => ml.ListId).HasColumnName("list_id");
            entity.Property(ml => ml.MediaId).HasMaxLength(50).HasColumnName("media_id");

            entity.HasOne(ml => ml.List)
                .WithMany(l => l.MediaListItems)
                .HasForeignKey(ml => ml.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ml => ml.Media)
                .WithMany(m => m.Lists)
                .HasForeignKey(ml => ml.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // PeopleListItem 
        modelBuilder.Entity<PeopleListItem>(entity =>
        {
            entity.ToTable("people_lists_rel");
            entity.HasKey(pl => new { pl.ListId, pl.PeopleId }).HasName("people_lists_rel_pkey");

            entity.Property(pl => pl.ListId).HasColumnName("list_id");
            entity.Property(pl => pl.PeopleId).HasMaxLength(50).HasColumnName("people_id");

            entity.HasOne(pl => pl.List)
                .WithMany(l => l.PeopleListItems)
                .HasForeignKey(pl => pl.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pl => pl.People)
                .WithMany(p => p.Lists)
                .HasForeignKey(pl => pl.PeopleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
