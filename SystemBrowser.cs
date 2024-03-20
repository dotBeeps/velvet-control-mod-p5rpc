using IdentityModel.OidcClient.Browser;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VelvetControl
{
    public class SystemBrowser
    {
        public int Port { get; }
        private readonly string _path;

        public SystemBrowser(int? port, string path)
        {
            _path = path;
            Port = port.Value;
        }

        public async Task<string?> InvokeAsync(string authUrl)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"{_path}:{Port}/");
            OpenBrowser(authUrl);
            try
            {
                var context = await listener.GetContextAsync();
                var req = context.Request;
                var resp = context.Response;
                using (var writer = new StreamWriter(resp.OutputStream))
                {
                    if (req.QueryString.AllKeys.Any("code".Contains))
                    {
                        return req.QueryString["code"];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true,
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
    }
}