using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataServiceLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaylistRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterDatabase()
            //     .Annotation("Npgsql:Enum:media_type", "tvShort,movie,tvMovie,short,tvMiniSeries,videoGame,tvEpisode,video,tvSpecial,tvSeries")
            //     .Annotation("Npgsql:Enum:title_type", "working,original,,imdbDisplayfestival,workingalternative,dvd,imdbDisplay,tv,imdbDisplaydvd,alternative,festival,video")
            //     .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'media_type') THEN
                    CREATE TYPE media_type AS ENUM ('tvShort','movie','tvMovie','short','tvMiniSeries','videoGame','tvEpisode','video','tvSpecial','tvSeries');
                END IF;
                IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'title_type') THEN
                    CREATE TYPE title_type AS ENUM ('working','original','imdbDisplayfestival','workingalternative','dvd','imdbDisplay','tv','imdbDisplaydvd','alternative','festival','video');
                END IF;
                END$$;
                ");


            migrationBuilder.CreateTable(
                name: "genres",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("genres_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    release_year = table.Column<int>(type: "integer", nullable: true),
                    end_year = table.Column<int>(type: "integer", nullable: true),
                    runtime_minutes = table.Column<int>(type: "integer", nullable: true),
                    imdb_average_rating = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true),
                    imdb_number_of_votes = table.Column<int>(type: "integer", nullable: true),
                    plot = table.Column<string>(type: "text", nullable: true),
                    awards = table.Column<string>(type: "text", nullable: true),
                    age_rating = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    poster = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    production = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    box_office = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    website_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    metascore = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    average_rating = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("media_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "people",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    birth_date = table.Column<int>(type: "integer", nullable: true),
                    death_date = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    name_rating = table.Column<decimal>(type: "numeric(3,1)", precision: 3, scale: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("people_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    hashed_password = table.Column<string>(type: "text", nullable: false),
                    email_verified = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    profile_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dvd_releases",
                columns: table => new
                {
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("dvd_releases_pkey", x => x.media_id);
                    table.ForeignKey(
                        name: "dvd_releases_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "episodes",
                columns: table => new
                {
                    episode_media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    series_media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    episode_number = table.Column<int>(type: "integer", nullable: true),
                    season_number = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("episodes_pkey", x => new { x.episode_media_id, x.series_media_id });
                    table.ForeignKey(
                        name: "episodes_episode_media_id_fkey",
                        column: x => x.episode_media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "episodes_series_media_id_fkey",
                        column: x => x.series_media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_genres",
                columns: table => new
                {
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    genre_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("media_genres_pkey", x => new { x.media_id, x.genre_id });
                    table.ForeignKey(
                        name: "media_genres_genre_id_fkey",
                        column: x => x.genre_id,
                        principalTable: "genres",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "media_genres_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_languages",
                columns: table => new
                {
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    language_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("media_languages_pkey", x => new { x.media_id, x.language_name });
                    table.ForeignKey(
                        name: "media_languages_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "titles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ordering = table.Column<int>(type: "integer", nullable: true),
                    region = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    language = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("titles_pkey", x => x.id);
                    table.ForeignKey(
                        name: "titles_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_people",
                columns: table => new
                {
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    people_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    known_for = table.Column<bool>(type: "boolean", nullable: true),
                    ordering = table.Column<int>(type: "integer", nullable: true),
                    job_note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    characters = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("media_people_pkey", x => new { x.media_id, x.people_id, x.role_id });
                    table.ForeignKey(
                        name: "media_people_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "media_people_people_id_fkey",
                        column: x => x.people_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "media_people_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "favorite_media",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("favorite_media_pkey", x => new { x.user_id, x.media_id });
                    table.ForeignKey(
                        name: "favorite_media_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "favorite_media_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "favorite_people",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    people_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("favorite_people_pkey", x => new { x.user_id, x.people_id });
                    table.ForeignKey(
                        name: "favorite_people_people_id_fkey",
                        column: x => x.people_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "favorite_people_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("lists_pkey", x => x.id);
                    table.ForeignKey(
                        name: "lists_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlists_pkey", x => x.id);
                    table.ForeignKey(
                        name: "playlists_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ratings_pkey", x => new { x.user_id, x.media_id });
                    table.ForeignKey(
                        name: "ratings_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "ratings_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recently_viewed",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    people_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("recently_viewed_pkey", x => x.id);
                    table.ForeignKey(
                        name: "recently_viewed_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "recently_viewed_people_id_fkey",
                        column: x => x.people_id,
                        principalTable: "people",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "recently_viewed_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "search_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    search_text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("search_history_pkey", x => x.id);
                    table.ForeignKey(
                        name: "search_history_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "watch_history",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("watch_history_pkey", x => new { x.user_id, x.media_id });
                    table.ForeignKey(
                        name: "watch_history_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "watch_history_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "title_attributes",
                columns: table => new
                {
                    title_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("title_attributes_pkey", x => x.title_id);
                    table.ForeignKey(
                        name: "title_attributes_title_id_fkey",
                        column: x => x.title_id,
                        principalTable: "titles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "media_lists_rel",
                columns: table => new
                {
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("media_lists_rel_pkey", x => new { x.list_id, x.media_id });
                    table.ForeignKey(
                        name: "media_lists_rel_list_id_fkey",
                        column: x => x.list_id,
                        principalTable: "lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "media_lists_rel_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "people_lists_rel",
                columns: table => new
                {
                    list_id = table.Column<Guid>(type: "uuid", nullable: false),
                    people_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("people_lists_rel_pkey", x => new { x.list_id, x.people_id });
                    table.ForeignKey(
                        name: "people_lists_rel_list_id_fkey",
                        column: x => x.list_id,
                        principalTable: "lists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "people_lists_rel_people_id_fkey",
                        column: x => x.people_id,
                        principalTable: "people",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "playlist_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    playlist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("playlist_items_pkey", x => x.id);
                    table.ForeignKey(
                        name: "playlist_items_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "playlist_items_playlist_id_fkey",
                        column: x => x.playlist_id,
                        principalTable: "playlists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    media_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    review_text = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("reviews_pkey", x => new { x.user_id, x.media_id });
                    table.ForeignKey(
                        name: "reviews_media_id_fkey",
                        column: x => x.media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "reviews_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "reviews_user_id_media_id_fkey",
                        columns: x => new { x.user_id, x.media_id },
                        principalTable: "ratings",
                        principalColumns: new[] { "user_id", "media_id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_episodes_series_media_id",
                table: "episodes",
                column: "series_media_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_media_media_id",
                table: "favorite_media",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_people_people_id",
                table: "favorite_people",
                column: "people_id");

            migrationBuilder.CreateIndex(
                name: "genres_name_key",
                table: "genres",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_lists_title",
                table: "lists",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "idx_lists_user_id",
                table: "lists",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_genres_genre_id",
                table: "media_genres",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_lists_rel_media_id",
                table: "media_lists_rel",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "idx_media_people_people_id",
                table: "media_people",
                column: "people_id");

            migrationBuilder.CreateIndex(
                name: "IX_media_people_role_id",
                table: "media_people",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "idx_people_name",
                table: "people",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_people_lists_rel_people_id",
                table: "people_lists_rel",
                column: "people_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_items_media_id",
                table: "playlist_items",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlist_items_playlist_id",
                table: "playlist_items",
                column: "playlist_id");

            migrationBuilder.CreateIndex(
                name: "IX_playlists_user_id",
                table: "playlists",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_ratings_media_id",
                table: "ratings",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "idx_recently_viewed_user_id",
                table: "recently_viewed",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recently_viewed_media_id",
                table: "recently_viewed",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "IX_recently_viewed_people_id",
                table: "recently_viewed",
                column: "people_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_media_id",
                table: "reviews",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "idx_roles_name",
                table: "roles",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_search_history_user_id",
                table: "search_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_titles_media_id",
                table: "titles",
                column: "media_id");

            migrationBuilder.CreateIndex(
                name: "idx_titles_title",
                table: "titles",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "idx_users_username",
                table: "users",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_watch_history_media_id",
                table: "watch_history",
                column: "media_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dvd_releases");

            migrationBuilder.DropTable(
                name: "episodes");

            migrationBuilder.DropTable(
                name: "favorite_media");

            migrationBuilder.DropTable(
                name: "favorite_people");

            migrationBuilder.DropTable(
                name: "media_genres");

            migrationBuilder.DropTable(
                name: "media_languages");

            migrationBuilder.DropTable(
                name: "media_lists_rel");

            migrationBuilder.DropTable(
                name: "media_people");

            migrationBuilder.DropTable(
                name: "people_lists_rel");

            migrationBuilder.DropTable(
                name: "playlist_items");

            migrationBuilder.DropTable(
                name: "recently_viewed");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "search_history");

            migrationBuilder.DropTable(
                name: "title_attributes");

            migrationBuilder.DropTable(
                name: "watch_history");

            migrationBuilder.DropTable(
                name: "genres");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "lists");

            migrationBuilder.DropTable(
                name: "playlists");

            migrationBuilder.DropTable(
                name: "people");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "titles");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "media");
        }
    }
}
