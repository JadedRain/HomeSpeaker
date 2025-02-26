using CommunityToolkit.Mvvm.ComponentModel;
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
	private ObservableCollection<SongModel> songs = new();

	[RelayCommand]
	public async void GetVolume()
	{
		Volume = await _homeSpeakerService.GetVolumeAsync();
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
	public async void PlayFirstSong()
	{

	}

	public HSHomeViewModel(HomeSpeakerService homeSpeakerService)
	{
		_homeSpeakerService = homeSpeakerService;
	}
}

