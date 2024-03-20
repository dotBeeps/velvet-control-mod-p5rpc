using System.Net.Http.Json;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using VelvetControl.Structs;

namespace VelvetControl;

internal class LoginFlow
{
    internal static async Task<string?> GetTwitchAuthCode()
    {
        var redir = "http://127.0.0.1:3000";
        var browser = new SystemBrowser(3000, "http://127.0.0.1");
        string authCodeUrl = string.Format($"https://id.twitch.tv/oauth2/authorize" +
                                           $"?response_type=code" +
                                           $"&client_id={"7u6rvbvy2nn0dsze7p32psunopxbj7"}" +
                                           $"&redirect_uri={redir}" +
                                           $"&scope=channel:read:redemptions" +
                                           $"&state={Util.BaseAddress.ToString()}");
        return await browser.InvokeAsync(authCodeUrl);
    }

    internal static async Task<string?> AuthWithServer(string authCode)
    {
        using (var client = new HttpClient())
        {
            try
            {
                VelvetControlAuth response =
                    await client.GetFromJsonAsync<VelvetControlAuth>(
                        $"https://velvet-control.beepboop.dog/auth?code={authCode}");
                return response.Token;
            }
            catch (HttpRequestException ex)
            {
                return $"{ex.StatusCode}: {ex.Message}";
            }
        } 
    }
}