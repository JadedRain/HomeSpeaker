using Grpc.Net.Client;
using HomeSpeaker.Shared;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.Services;

public class HomeSpeakerService : HomeSpeakerBase
{
	private readonly ILogger<HomeSpeakerService> _logger;
	private HomeSpeakerClient _client;
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
}
