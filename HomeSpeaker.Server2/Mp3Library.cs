﻿using HomeSpeaker.Shared;

namespace HomeSpeaker.Server
{
    public class Mp3Library
    {
        private readonly IFileSource fileSource;
        private readonly ITagParser tagParser;
        private readonly IDataStore dataStore;
        private readonly ILogger<Mp3Library> logger;

        public Mp3Library(IFileSource fileSource, ITagParser tagParser, IDataStore dataStore, ILogger<Mp3Library> logger)
        {
            this.fileSource = fileSource ?? throw new ArgumentNullException(nameof(fileSource));
            this.tagParser = tagParser ?? throw new ArgumentNullException(nameof(tagParser));
            this.dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            logger.LogInformation($"Initialized with fileSource {fileSource.RootFolder}");

            if (SyncStarted is false)
            {
                SyncLibrary();
            }
        }

        public string RootFolder => fileSource.RootFolder;

        public bool SyncStarted { get; private set; }
        public bool SyncCompleted { get; private set; }

        public void SyncLibrary()
        {
            if (SyncStarted)
            {
                return;
            }

            SyncStarted = true;
            var files = fileSource.GetAllMp3s();
            foreach (var file in files)
            {
                try
                {
                    var song = tagParser.CreateSong(file);
                    dataStore.Add(song);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Trouble parsing tag info!");
                }
            }
            SyncCompleted = true;
            logger.LogInformation($"Sync Completed! {dataStore.GetSongs().Count():n0} songs in database.");
        }

        public IEnumerable<Artist> Artists => dataStore.GetArtists();
        public IEnumerable<Album> Albums => dataStore.GetAlbums();
        public IEnumerable<Song> Songs => dataStore.GetSongs();

        public void ResetLibrary()
        {
            dataStore.Clear();
            SyncLibrary();
        }
    }
}
