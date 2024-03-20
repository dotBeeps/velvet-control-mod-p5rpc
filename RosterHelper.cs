using System.Diagnostics;
using p5rpc.lib;
using Reloaded.Memory.Pointers;
using Reloaded.Mod.Interfaces;
using VelvetControl.Configuration;
using VelvetControl.Structs;
using System.Linq;
using Newtonsoft.Json;

namespace VelvetControl;

public class RosterHelper
{
       public static FixedArrayPtr<PersonaStruct> Personas;
       private static ILogger _logger;
       private static Config _config;

       internal static unsafe void Init(ILogger logger, Config config)
       { 
              _logger = logger;
              _config = config;
              var baseAddress = (long) Process.GetCurrentProcess().MainModule.BaseAddress;
              var rosterAddress = baseAddress + 0x29E8EE4;
              Personas = new FixedArrayPtr<PersonaStruct>((PersonaStruct*)rosterAddress, 12);
              logger.WriteLine($"[Velvet Control] Roster Helper started.");
       }

       internal static void DebugRoster()
       {
              for (int i = 0; i < Personas.Count; i++)
              {
                     PersonaStruct pstr = Personas.Get(i);
                     _logger.WriteLine($"{JsonConvert.SerializeObject(pstr, Formatting.Indented)}");
              }
       }

       internal static void ClearSlot(int slot)
       {
              for (int i = slot; i < 11; i++)
              {
                     PersonaStruct p = Personas.Get(i + 1);
                     Personas.Set(i,p);
              }
              Personas.Set(11,new PersonaStruct());
       }

       internal static void SetSlot(int slot, PersonaStruct pStruct)
       {
              PersonaStruct replace = Personas.Get(slot);
              if (replace.Id < 1)
              {
                     while (replace.Id < 1)
                     {
                            slot--;
                            replace = Personas.Get(slot);
                     }
                     slot++;
              }
              Personas.Set(slot,pStruct);
       }
}