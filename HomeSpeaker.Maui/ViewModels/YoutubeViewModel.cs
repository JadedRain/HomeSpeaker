using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.Maui.Services;
using HomeSpeaker.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static HomeSpeaker.Shared.HomeSpeaker;
using Video = HomeSpeaker.Shared.Video;


namespace HomeSpeaker.Maui.ViewModels;

public partial class YoutubeViewModel : ObservableObject
{
    private HomeSpeakerService _hss;
    private MauiYoutubeService _mauiYoutubeService;

    bool isSearching = false;

    [ObservableProperty]
    private ObservableCollection<Video> videos;

    [ObservableProperty]
    private string searchTerm = "";

    public YoutubeViewModel(HomeSpeakerService hss, MauiYoutubeService mys)
    {
        _hss = hss;
        _mauiYoutubeService = mys;
    }

    [RelayCommand]
    public async Task SearchYoutube()
    {
        isSearching = true;
        var response = await _hss.HomeSpeakerClient.SearchViedoAsync(new SearchVideoRequest { SearchTerm = SearchTerm });
        Videos = new ObservableCollection<Video>(response.Results.ToList());
        isSearching = false;

    }

    [RelayCommand]
    public async Task DownloadVideo(Video video)
    {
        // The current issue that happens is files save to bin/Debug/...
        // Don't know how this saves for you but this is a reminder to myself about what is happening. Will solve. (Probably)
        if (video != null)
        {
            try
            {
                // Calling the CacheVideoAsync method to download the video
                await _mauiYoutubeService.CacheVideoAsync(video.Id, video.Title, new Progress<double>(progress =>
                {
                    System.Diagnostics.Debug.WriteLine($"Download progress: {progress}%");
                }));

                System.Diagnostics.Debug.WriteLine($"Download of {video.Title} completed.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error during download: {ex.Message}");
            }
        }
    }
    [RelayCommand]
    private async Task NavigateToYoutube()
    {
        await Shell.Current.GoToAsync("///YoutubeView");
    }
    [RelayCommand]
    private async Task NavigateToSongs()
    {
        await Shell.Current.GoToAsync("///SongsView");
    }
    [RelayCommand]
    private async Task NavigateToClientManagment()
    {
        await Shell.Current.GoToAsync("///ClientManagementView");
    }
    [RelayCommand]
    private async Task NavigateToHome()
    {
        await Shell.Current.GoToAsync("///HSHomeView");
    }
}

