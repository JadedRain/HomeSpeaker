using HomeSpeaker.Shared;
using Id3;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace HomeSpeaker.Server
{
    public interface ITagParserMaui
    {
        Song CreateSong(string fullPath);
    }

    public class DefaultTagParser : ITagParserMaui
    {
        private readonly ILogger<DefaultTagParser> logger;

        public DefaultTagParser(ILogger<DefaultTagParser> logger)
        {
            this.logger = logger;
        }

        public Song CreateSong(string fullPath)
        {
            var fileName = Path.GetFileName(fullPath);
            using var mp3 = new Mp3(fullPath);
            var tag = mp3.GetTag(Id3TagFamily.Version2X) ?? mp3.GetTag(Id3TagFamily.Version1X) ?? throw new ApplicationException("Unable to find MP3 tags for " + fullPath);
            var title = tag.Title?.Value?.Replace("\0", string.Empty) ?? string.Empty;
            if (title.Length == 0)
            {
                title = fileName.Replace(".mp3", string.Empty);
            }
            return new Song
            {
                Album = tag.Album.Value?.Replace("\0", string.Empty),
                Artist = tag.Artists.Value.FirstOrDefault()?.Replace("\0", string.Empty) ?? "[Artist Unknown]",
                Name = title,
                Path = fullPath
            };
        }
    }
}
