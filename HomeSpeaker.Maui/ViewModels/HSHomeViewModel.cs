﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.Maui.Models;
using HomeSpeaker.Maui.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.ViewModels;

public partial class HSHomeViewModel : ObservableObject
{
	private HomeSpeakerService _homeSpeakerService;

	[ObservableProperty]
	private int volume = 1;

	[ObservableProperty]
	private SongModel currentSong;


	[ObservableProperty]
	private ObservableCollection<SongModel> songs = new();


	[RelayCommand]
	public async void GetVolume()
	{
		Volume = await _homeSpeakerService.GetVolumeAsync();
	}

    [RelayCommand]
    public async Task UpdateVolume(int newVolume)
    {   
            Volume = newVolume;
            await _homeSpeakerService.SetVolumeAsync(newVolume);
    }

	partial void OnVolumeChanged(int oldValue, int newValue)
	{
		UpdateVolume(newValue);
	}

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


	[RelayCommand]
	public async Task PlaySong(SongModel song)
	{
		if (song == null) return;
		await SetCurrentSong(song.SongId);
		await _homeSpeakerService.PlaySongAsync(song.SongId);
	}

    [RelayCommand]
    public async Task PlaySongFolder()
    {
        // Ensure that there are songs in the collection before attempting to play
        if (Songs.Any())
        {
            // Get the folder path from the first song (or use any logic to get the folder path)
            var firstSongFolderPath = Songs.FirstOrDefault()?.Folder;

            // If a folder path is found, play the folder
            if (!string.IsNullOrEmpty(firstSongFolderPath))
            {
                await _homeSpeakerService.PlayFolderAsync(firstSongFolderPath);
            }
        }
      
    }

    [RelayCommand]
	public async Task PauseSong()
	{
		await _homeSpeakerService.StopPlayingAsync();
		await SetCurrentSong(-1);
	}

	[RelayCommand]
	public async Task ResumeSong()
	{
        await _homeSpeakerService.ResumePlayAsync();
    }
    [RelayCommand]
    public async Task ClearQueue()
    {
            await _homeSpeakerService.ClearQueueAsync();
            Songs.Clear();  
    }


    public HSHomeViewModel(HomeSpeakerService homeSpeakerService)
	{
		_homeSpeakerService = homeSpeakerService;
        GetVolume();
		GetSongs();
	}

	private async Task SetCurrentSong(int songId)
	{
		if (songId == -1 || songId == null)
		{
			CurrentSong = null;
			return;
		}
		CurrentSong = Songs.Where(song => song.SongId == songId).FirstOrDefault();
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

