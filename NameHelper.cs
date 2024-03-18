using System.Diagnostics;
using p5rpc.lib;
using Reloaded.Memory.Pointers;
using Reloaded.Mod.Interfaces;
using VelvetControl.Configuration;
using VelvetControl.Structs;
using System.Linq;
using Newtonsoft.Json;

namespace VelvetControl;

internal class NameHelper
{
       private static unsafe Names* Names;
       private static ILogger _logger;
       private static Config _config;

       internal static unsafe void Init(ILogger logger, Config config)
       { 
              _logger = logger;
              _config = config;
              var address = Util.BaseAddress + 0x29FC626;
              Names = (Names*)address;
       }

       internal static unsafe void SetFirstName(string name)
       {
              Names->FirstName = name;
              Names->FullName = $"{Names->FirstName} {Names->LastName}";
       }
       
       internal static unsafe void SetLastName(string name)
       {
              Names->LastName = name;
              Names->FullName = $"{Names->FirstName} {Names->LastName}";
       }

       internal static unsafe void SetTeamName(string name)
       {
              Names->GroupName = name;
       }
}