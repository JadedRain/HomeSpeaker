using HomeSpeaker.Maui.ViewModels;
namespace HomeSpeaker.Maui.Views;

public partial class ClientManagementView : ContentPage
{
	public ClientManagementView(ClientManagementViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}