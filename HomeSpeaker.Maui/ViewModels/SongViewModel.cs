using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.Maui.Models;
using HomeSpeaker.Maui.Services;
using System.Collections.ObjectModel;

namespace HomeSpeaker.Maui.ViewModels;

public partial class SongViewModel : ObservableObject
{

    private HomeSpeakerService _homeSpeakerService;

    [ObservableProperty]
    private ObservableCollection<SongModel> songs = new();

    [RelayCommand]
    public async void GetSongs()
    {
        Songs.Clear();
        var allSongs = await _homeSpeakerService.GetAllSongsAsync();
        foreach (var song in allSongs)
        {
            Songs.Add(song);
        }
        OnPropertyChanged(nameof(Songs));
    }
}
