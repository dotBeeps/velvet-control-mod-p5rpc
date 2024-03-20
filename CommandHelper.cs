using VelvetControl.Structs;

namespace VelvetControl;

public class CommandHelper
{
    public Dictionary<string, Action<VelvetParams>> Commands = new();
    private readonly PersonaHelper _personaHelper;
    
    public CommandHelper(PersonaHelper personaHelper)
    {
        _personaHelper = personaHelper;
        RegisterCommands();
    }

    private void RegisterCommands()
    {
        Commands.Add("give_persona", GivePersona);
        Commands.Add("start_battle", StartBattle);
        Commands.Add("set_player_level", SetPlayerLevel);
        Commands.Add("call_event", CallEvent);
        Commands.Add("set_player_model", SetPlayerModel);
        Commands.Add("call_field", CallField);
        Commands.Add("spawn_npc", SpawnNpc);
        Commands.Add("delete_persona", DeletePersona);
        Commands.Add("set_persona", SetPersona);
        Commands.Add("transmute_persona", TransmutePersona);
        Commands.Add("set_ailment", SetAilments);
    }

    private void GivePersona(VelvetParams para)
    {
        _personaHelper.GivePersona(para["id"]);
    }

    private void StartBattle(VelvetParams para)
    {
        _personaHelper.LoadEncounter(para["encounter"]);
    }

    private void SetPlayerLevel(VelvetParams para)
    {
        _personaHelper.ModifyPlayerLevel(1, para["level"]);
    }

    private void SetPlayerModel(VelvetParams para)
    {
        _personaHelper.SetPlayerModel(para["start"], para["middle"], para["end"]);
    }

    private void CallField(VelvetParams para)
    {
        _personaHelper.CallField(para["major"], para["minor"], para["entrance"], para["positionId"]);
    }

    private void CallEvent(VelvetParams para)
    {
        int id1 = para["id1"];
        int id2 = para["id2"];
        _personaHelper.CallEvent(id1, id2);
    }

    private void SpawnNpc(VelvetParams para)
    {
        _personaHelper.NpcSpawn(para["idMajor"], para["idMinor"], para["idSub"]);
    }

    private void DeletePersona(VelvetParams para)
    {
        RosterHelper.ClearSlot(para["slot"]);
    }

    private void SetPersona(VelvetParams para)
    {
        PersonaStruct personaStruct = new PersonaStruct()
        {
            Registered = 1,
            Id = (ushort)para["id"], Level = (byte)para["level"], Traits = (ushort)para["traits"], Exp = para["xp"],
            St = (byte)para["st"], Ma = (byte)para["ma"], En = (byte)para["en"], Ag = (byte)para["ag"], Lu = (byte)para["lu"],
            SkillOne = (ushort)para.GetValueOrDefault("skill1",0), 
            SkillTwo = (ushort)para.GetValueOrDefault("skill2",0),
            SkillThree = (ushort)para.GetValueOrDefault("skill3",0),
            SkillFour = (ushort)para.GetValueOrDefault("skill4",0),
            SkillFive = (ushort)para.GetValueOrDefault("skill5",0),
            SkillSix = (ushort)para.GetValueOrDefault("skill6",0),
            SkillSeven = (ushort)para.GetValueOrDefault("skill7",0),
            SkillEight = (ushort)para.GetValueOrDefault("skill8",0),
        };
        RosterHelper.SetSlot(para["slot"], personaStruct);
    }

    private void TransmutePersona(VelvetParams para)
    {
        PersonaStruct persona = RosterHelper.Personas.Get(para["slot"]);
        persona.Id = (ushort) para["id"];
        RosterHelper.Personas.Set(para["slot"], persona);
    }

    private void SetAilments(VelvetParams para)
    {
        PartyStatusHelper.SetStatus(para["slot"], (uint)para["ailment"]);
    }
}