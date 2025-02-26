using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using HomeSpeaker.Maui.Services;
using System.Threading.Tasks;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.ViewModels;

public partial class HSHomeViewModel: ObservableObject
{
    [ObservableProperty]
    private int volume = 1;

    [RelayCommand]
    public async Task GetVolume()
    {
        GrpcClientFactory.AllowUnencryptedHttp2 = true;
        using var channel= GrpcChannel.ForAddress("http://localhost:5280");
        var client = channel.CreateGrpcService<HomeSpeakerService1>();
        var Response = await client.GetVolumeAsync();
        Volume = Response;

    }

}
