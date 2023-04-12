﻿using Microsoft.EntityFrameworkCore;

namespace HomeSpeaker.Server2.Data;

public class MusicContext : DbContext
{
    public MusicContext(DbContextOptions<MusicContext> options) : base(options)
    {

    }
    public DbSet<Thumbnail> Thumbnails { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistItem> PlaylistItems { get; set; }
    public DbSet<Impression> Impressions { get; set; }
}

public class Thumbnail
{
    public int Id { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public string ThumbnailUrl { get; set; }
}

public class Playlist
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PlaylistItem> Songs { get; set; } = new();
}

public class PlaylistItem
{
    public int Id { get; set; }
    public int PlaylistId { get; set; }
    public string SongPath { get; set; }
    public int Order { get; set; }
}

public class Impression
{
    public int Id { get; set; }
    public string SongPath { get; set; }
    public DateTime Timestamp { get; set; }
    public string PlayedBy { get; set; }
}