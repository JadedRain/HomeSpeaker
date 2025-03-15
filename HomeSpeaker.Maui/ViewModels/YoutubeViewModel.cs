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
    private readonly MauiYoutubeService _mauiYoutubeService;
    private HomeSpeakerService _hss;
    bool isSearching = false;

    [ObservableProperty]
    private ObservableCollection<Video> videos;

    [ObservableProperty]
    private string searchTerm = "";

    public YoutubeViewModel(HomeSpeakerService svc)
    {
        this._hss = svc;
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
        if (video == null)
        {
            return;
        }

        var progress = new Progress<double>(percentage =>
        {
            // You can update the UI here with the download progress percentage
            Console.WriteLine($"Download Progress: {percentage}%");
        });

        try
        {
            await _mauiYoutubeService.CacheVideoAsync(video.Id, video.Title, progress);
        }
        catch (Exception ex)
        {
            return;
        }
    }

}
