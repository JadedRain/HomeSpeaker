using HomeSpeaker.Maui.ViewModels;

namespace HomeSpeaker.Maui.Views;

public partial class HSHomeView : ContentPage
{
	public HSHomeView()
	{
		InitializeComponent();
        BindingContext = new HSHomeViewModel();
    }
}