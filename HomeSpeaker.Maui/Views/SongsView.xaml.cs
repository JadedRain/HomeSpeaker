using HomeSpeaker.Maui.ViewModels;

namespace HomeSpeaker.Maui.Views;

public partial class SongsView : ContentPage
{
	public SongsView(SongViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}