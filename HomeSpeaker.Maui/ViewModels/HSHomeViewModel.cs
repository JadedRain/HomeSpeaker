using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.Maui.Services;
using System.Threading.Tasks;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.ViewModels;

public partial class HSHomeViewModel : ObservableObject
{
	private HomeSpeakerService _homeSpeakerService;

	[ObservableProperty]
	private int volume = 1;

	[ObservableProperty]
	private string testString;

	[RelayCommand]
	public void SendTest()
	{
		TestString = _homeSpeakerService.SendTest();
	}

	public HSHomeViewModel(HomeSpeakerService homeSpeakerService)
	{
		_homeSpeakerService = homeSpeakerService;
	}
}

