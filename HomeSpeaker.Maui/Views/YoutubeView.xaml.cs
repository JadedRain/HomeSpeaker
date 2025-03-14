using HomeSpeaker.Maui.ViewModels;

namespace HomeSpeaker.Maui.Views;

public partial class YoutubeView : ContentPage
{
	public YoutubeView(YoutubeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}