

using HomeSpeaker.Shared;
using static HomeSpeaker.Shared.HomeSpeaker;

namespace HomeSpeaker.Maui.Services;

class HomeSpeakerService1
{
    private HomeSpeakerClient client;

    public async Task SetVolumeAsync(int volume0to100)
    {
        var request = new PlayerControlRequest { SetVolume = true, VolumeLevel = volume0to100 };
        await client.PlayerControlAsync(request);
    }

    public async Task<int> GetVolumeAsync()
    {
        //var status = await GetStatusAsync();
        return 5;
    }
    public async Task<GetStatusReply> GetStatusAsync()
    {
        return await client.GetPlayerStatusAsync(new GetStatusRequest());
    }
}
