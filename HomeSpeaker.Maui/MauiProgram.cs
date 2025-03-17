using CommunityToolkit.Maui;
using HomeSpeaker.Maui.Services;
using HomeSpeaker.Maui.ViewModels;
using HomeSpeaker.Maui.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using HomeSpeaker.Shared;

namespace HomeSpeaker.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		//builder.Services.AddSingleton<ILogger<HomeSpeakerService>>();
		builder.Services.AddSingleton<HomeSpeakerService>();
		builder.Services.AddSingleton<HSHomeViewModel>();
		builder.Services.AddSingleton<HSHomeView>();
		builder.Services.AddSingleton<SongViewModel>();
		builder.Services.AddSingleton<SongsView>();
		builder.Services.AddSingleton<ClientManagementViewModel>();
		builder.Services.AddSingleton<ClientManagementView>();
		//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
		builder.Services.AddSingleton<IDataStoreMaui, OnDiskDataStoreMaui>();
		builder.Services.AddSingleton<IFileSourceMaui>(_ => new DefaultFileSource("HomeSpeakerMedia"));
		builder.Services.AddSingleton<ITagParserMaui, DefaultTagParser>();

		builder.Services.AddSingleton<Mp3LibraryMaui>();
        builder.Services.AddSingleton<MauiYoutubeService>();
		builder.Services.AddSingleton<YoutubeViewModel>();
		builder.Services.AddSingleton<YoutubeView>();

        return builder.Build();
	}
}
// Temp:
// builder.Configuration[ConfigKeysMaui.MediaFolder] ?? throw new MissingConfigExceptionMaui(ConfigKeysMaui.MediaFolder))