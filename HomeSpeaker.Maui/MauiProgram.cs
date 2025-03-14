﻿using CommunityToolkit.Maui;
using HomeSpeaker.Maui.Services;
using HomeSpeaker.Maui.ViewModels;
using HomeSpeaker.Maui.Views;
using Microsoft.Extensions.Logging;

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
        builder.Services.AddSingleton<MauiYoutubeService>();
		builder.Services.AddSingleton<YoutubeViewModel>();
		builder.Services.AddSingleton<YoutubeView>();

        return builder.Build();
	}
}
