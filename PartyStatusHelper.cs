using System.Diagnostics;
using p5rpc.lib;
using Reloaded.Memory.Pointers;
using Reloaded.Mod.Interfaces;
using VelvetControl.Configuration;
using VelvetControl.Structs;
using System.Linq;
using Newtonsoft.Json;

namespace VelvetControl;

public class PartyStatusHelper
{
       internal static FixedArrayPtr<BattleStatus> Party;
       internal static ILogger _logger;
       private static Config _config;

       internal static unsafe void Init(ILogger logger, Config config)
       { 
              _logger = logger;
              _config = config;
              var baseAddress = (long) Process.GetCurrentProcess().MainModule.BaseAddress;
              var partyAddress = baseAddress + 0x29E8EAC;
              logger.WriteLine($"[Velvet Control] Hooked party status at {partyAddress}");
              Party = new FixedArrayPtr<BattleStatus>((BattleStatus*)partyAddress,10);
       }

       internal static void DebugParty()
       {
              for (int i = 0; i < Party.Count; i++)
              {
                     BattleStatus partyMem = Party.Get(i);
                     _logger.WriteLine($"{JsonConvert.SerializeObject(partyMem, Formatting.Indented)}");
                     _logger.WriteLine($"Level {partyMem.Level}");
                     _logger.WriteLine($"Exp: {partyMem.Exp}");
                     _logger.WriteLine($"HP: {partyMem.HP}");
                     _logger.WriteLine($"SP: {partyMem.SP}");
                     _logger.WriteLine($"Ailments: {partyMem.Ailments}");
                     _logger.WriteLine($"Persona: {JsonConvert.SerializeObject(partyMem.Persona, Formatting.Indented)}");
              }
       }
       
       internal static void SetStatus(int slot, uint ailment)
       {
              BattleStatus partyMember = Party.Get(slot);
              partyMember.Ailments = ailment;
              Party.Set(slot, partyMember);
       }
}