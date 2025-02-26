using HomeSpeaker.Shared;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.Services;

public class HomeSpeakerService : HomeSpeakerBase
{
	private readonly ILogger<HomeSpeakerService> _logger;
	public HomeSpeakerService(ILogger<HomeSpeakerService> logger)
	{
		_logger = logger ?? throw new System.ArgumentNullException(nameof(logger)); ;
	}

	public string SendTest()
	{
		return "Just a test";
	}
}
