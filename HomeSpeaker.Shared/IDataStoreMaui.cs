using HomeSpeaker.Shared;
using System.Collections.Generic;

namespace HomeSpeaker.Server2;

public interface IDataStoreMaui
{
    void Add(Song song);
    IEnumerable<Artist> GetArtists();
    IEnumerable<Album> GetAlbums();
    IEnumerable<Song> GetSongs();
    void Clear();
}
