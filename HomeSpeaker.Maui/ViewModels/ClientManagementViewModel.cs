using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HomeSpeaker.Maui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSpeaker.Maui.ViewModels;

public partial class ClientManagementViewModel : ObservableObject
{
	private HomeSpeakerService _homeSpeakerService;

	[ObservableProperty]
	private string currentClientAddress;

	[ObservableProperty]
	private string updatedClientAddress;

	[RelayCommand]
	public async void SetCurrentClientAddress()
	{
		await _homeSpeakerService.UpdateClient(UpdatedClientAddress);
		CurrentClientAddress = UpdatedClientAddress;
	}
	public ClientManagementViewModel(HomeSpeakerService homeSpeakerService) 
	{ 
		_homeSpeakerService = homeSpeakerService;
		CurrentClientAddress = _homeSpeakerService.defaultAddress;
	}
}
