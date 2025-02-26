using HomeSpeaker.Maui.ViewModels;

namespace HomeSpeaker.Maui.Views;

public partial class HSHomeView : ContentPage
{
	public HSHomeView(HSHomeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}