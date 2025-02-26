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

	public IEnumerable<SongMessage> Songs => songs;
	public HomeSpeakerService(ILogger<HomeSpeakerService> logger)
	{
		_logger = logger;
		var address = "https://localhost:7238";
		var channel = GrpcChannel.ForAddress(address);

		_client = new HomeSpeakerClient(channel);

	}
	public HomeSpeakerClient HomeSpeakerClient => _client;
	public async Task<int> GetVolumeAsync()
	{
		_logger.LogInformation("Started Get Volume");
		var status = await GetStatusAsync();
		_logger.LogInformation("Got status");
		return status.Volume;
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

}
