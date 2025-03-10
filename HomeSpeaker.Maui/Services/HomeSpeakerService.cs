using Grpc.Net.Client;
using HomeSpeaker.Shared;
using HomeSpeaker.Maui.Models;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static HomeSpeaker.Shared.HomeSpeaker;
using Grpc.Core;

namespace HomeSpeaker.Maui.Services;

public class HomeSpeakerService : HomeSpeakerBase
{
	private readonly ILogger<HomeSpeakerService> _logger;
	private HomeSpeakerClient _client;
	private List<SongMessage> songs = new();
    public event EventHandler QueueChanged;


    public IEnumerable<SongMessage> Songs => songs;
	public HomeSpeakerService(ILogger<HomeSpeakerService> logger)
	{
		_logger = logger;
		var address = "https://localhost:7238";
		var channel = GrpcChannel.ForAddress(address);

		_client = new HomeSpeakerClient(channel);
        _ = listenForEvents();

    }

    private async Task listenForEvents()
    {
        var eventReply = _client.SendEvent(new Google.Protobuf.WellKnownTypes.Empty());
        await foreach (var eventInstance in eventReply.ResponseStream.ReadAllAsync())
        {
            StatusChanged?.Invoke(this, eventInstance.Message);
        }
    }
    public HomeSpeakerClient HomeSpeakerClient => _client;
	public async Task<int> GetVolumeAsync()
	{
		_logger.LogInformation("Started Get Volume");
		var status = await GetStatusAsync();
		_logger.LogInformation("Got status");
		return status.Volume;
	}

    public async Task UpdateQueueAsync(List<SongModel> songs)
    {
        var request = new UpdateQueueRequest();
        request.Songs.AddRange(songs.Select(s => s.Path));
        await _client.UpdateQueueAsync(request);
    }
    public async Task<GetStatusReply> GetStatusAsync()
	{
		return await _client.GetPlayerStatusAsync(new GetStatusRequest());
	}

	public async Task SetVolumeAsync(int volume0to100)
	{
		var request = new PlayerControlRequest { SetVolume = true, VolumeLevel = volume0to100 };
		await _client.PlayerControlAsync(request);
	}

    public async Task EnqueueFolderAsync(SongGroup songs) => await _client.EnqueueFolderAsync(new EnqueueFolderRequest { FolderPath = songs.FolderPath });

    public async Task<IEnumerable<SongModel>> GetSongsInFolder(string folder)
    {
        var songs = new List<SongModel>();
        var getSongsReply = _client.GetSongs(new GetSongsRequest { Folder = folder });
        await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
        {
            songs.AddRange(reply.Songs.Select(s => s.ToSongModel()));
        }

        return songs;
    }

    public async Task<Dictionary<string, List<SongModel>>> GetSongGroups()
    {
        var groups = new Dictionary<string, List<SongModel>>();
        var getSongsReply = _client.GetSongs(new GetSongsRequest { });
        await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
        {
            foreach (var s in reply.Songs)
            {
                var song = s.ToSongModel();
                if (song.Folder == null)
                {
                    continue;
                }

                if (groups.ContainsKey(song.Folder) is false)
                {
                    groups[song.Folder] = new List<SongModel>();
                }

                groups[song.Folder].Add(song);
            }
        }

        return groups;
    }

    public async Task<IEnumerable<SongModel>> GetAllSongsAsync()
	{
			_logger.LogWarning("Trying to send otel trace!");
			var songs = new List<SongModel>();
			var getSongsReply = _client.GetSongs(new GetSongsRequest { });
			await foreach (var reply in getSongsReply.ResponseStream.ReadAllAsync())
			{
				songs.AddRange(reply.Songs.Select(s => s.ToSongModel()));
			}
			return songs;
	}

	public async Task PlaySongAsync(int songId)
	{
		await _client.PlayerControlAsync(new PlayerControlRequest { Stop = true, ClearQueue = true });
		await _client.EnqueueSongAsync(new PlaySongRequest { SongId = songId });
	}

    public async Task EnqueueSongAsync(int songId) => await _client.EnqueueSongAsync(new PlaySongRequest { SongId = songId });
    public async Task PlayFolderAsync(string folder) => await _client.PlayFolderAsync(new PlayFolderRequest { FolderPath = folder });
    public async Task EnqueueFolderAsync(string folder) => await _client.EnqueueFolderAsync(new EnqueueFolderRequest { FolderPath = folder });
    public async Task StopPlayingAsync() => await _client.PlayerControlAsync(new PlayerControlRequest { Stop = true });

    public async Task ResumePlayAsync()
    {
        var status = await GetStatusAsync();

        await _client.PlayerControlAsync(new PlayerControlRequest { Play = true });
		QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task ClearQueueAsync()
    {
        await _client.PlayerControlAsync(new PlayerControlRequest { ClearQueue = true });
        QueueChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler<string>? StatusChanged;

}
