using System.Diagnostics;
using Reloaded.Mod.Interfaces;
using VelvetControl.Configuration;

namespace VelvetControl;

internal static class Util
{
    private static ILogger _logger;
    private static Config _config;
    internal static long BaseAddress;
    
    internal static unsafe void Init(ILogger logger, Config config)
    { 
        _logger = logger;
        _config = config;
        BaseAddress = Process.GetCurrentProcess().MainModule.BaseAddress;
    }
}