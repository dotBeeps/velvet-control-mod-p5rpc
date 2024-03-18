using p5rpc.lib.interfaces;
using Reloaded.Mod.Interfaces;

namespace VelvetControl;

public class PersonaHelper
{
    private readonly IP5RLib _p5RLib;
    private readonly IFlowCaller _flowCaller;
    private List<int> playerLevel;
    private readonly ILogger _logger;

    private bool inEvents = false;

    private int fldMaj = 0;
    private int fldMin = 100;
    private int fldEntrance = 0;
    private int fldPositionId = 0;
    
    public PersonaHelper(IP5RLib p5rLib, ILogger logger)
    {
        _p5RLib = p5rLib;
        _logger = logger;
        _flowCaller = _p5RLib.FlowCaller;
    }

    public int GetPlayerLevel(int index)
    {
        return _flowCaller.GET_PC_LEVEL(index);
    }

    public void ModifyPlayerLevel(int index, int lvl)
    {
        _flowCaller.SET_HUMAN_LV(index,lvl);
    }

    public void LoadEncounter(int encounterId)
    {
        int fldMaj = _flowCaller.FLD_GET_MAJOR();
        int fldMin = _flowCaller.FLD_GET_MINOR();
        int fldEntrance = _flowCaller.FLD_GET_DIV_INDEX();
        int fldPositionId = _flowCaller.FLD_GET_POS_INDEX();
        _flowCaller.CALL_BATTLE( encounterId ); 
        Thread.Sleep(100);
        _flowCaller.WAIT_BATTLE();
        _flowCaller.CALL_FIELD(fldMaj,fldMin,fldEntrance,fldPositionId);
    }

    public void GivePersona(int id)
    {
        _flowCaller.ADD_PERSONA_STOCK(id);
        _flowCaller.SET_MSG_VAR(0, id, 7);
    }

    public void SystemMessage(int messageId)
    {
        _flowCaller.MSG_WND_DSP();
        //_flowCaller.MSG_SYSTEM(messageId);
        _flowCaller.MSG_WND_CLS();
    }

    public void AIUseSkill(int skillId)
    {
        _flowCaller.AI_ACT_SKILL(skillId);
    }

    public void CallEvent(int id1, int id2)
    {
        Thread.Sleep(100);
        int check = _flowCaller.FLD_GET_MAJOR();
        if (check >= 0)
        {
            fldMaj = _flowCaller.FLD_GET_MAJOR();
            fldMin = _flowCaller.FLD_GET_MINOR();
            fldEntrance = _flowCaller.FLD_GET_DIV_INDEX();
            fldPositionId = _flowCaller.FLD_GET_POS_INDEX();
        }
        _logger.WriteLine($"{fldMaj} {fldMin} {fldEntrance} {fldPositionId}");
        _flowCaller.CALL_EVENT(id1,id2);
        Thread.Sleep(50);
        _flowCaller.CALL_FIELD(fldMaj,fldMin,fldEntrance,fldPositionId);
    }

    public void CallField(int maj, int min, int entrance, int positionId)
    {
        _flowCaller.CALL_FIELD(maj,min,entrance,positionId);
    }
    
    public void SetPlayerModel(int start, int middle, int end)
    {
        _flowCaller.SET_COUNT(18, start+middle+end);
        Thread.Sleep(50);
        reloadField();
    }

    private void reloadField()
    {
        fldMaj = _flowCaller.FLD_GET_MAJOR();
        fldMin = _flowCaller.FLD_GET_MINOR();
        fldEntrance = _flowCaller.FLD_GET_DIV_INDEX();
        fldPositionId = _flowCaller.FLD_GET_POS_INDEX();
        _flowCaller.CALL_FIELD(fldMaj,fldMin,fldEntrance,fldPositionId);
    }

    public void NpcSpawn(int idMajor, int idMinor, int idSub)
    {
        int pcHandle = _flowCaller.FLD_PC_GET_RESHND(0);
        while (pcHandle == -1)
        {
            Thread.Sleep(100);
            pcHandle = _flowCaller.FLD_PC_GET_RESHND(0);
        }
        _logger.WriteLine($"PC Handle: {pcHandle}");
        float x = _flowCaller.FLD_MODEL_GET_X_TRANSLATE(pcHandle);
        float y = _flowCaller.FLD_MODEL_GET_Y_TRANSLATE(pcHandle);
        float z = _flowCaller.FLD_MODEL_GET_Z_TRANSLATE(pcHandle);
        float xr = _flowCaller.FLD_MODEL_GET_X_ROTATE(pcHandle);
        float yr = _flowCaller.FLD_MODEL_GET_Y_ROTATE(pcHandle);
        float zr = _flowCaller.FLD_MODEL_GET_Z_ROTATE(pcHandle);
        int npcHandle = _flowCaller.FLD_NPC_MODEL_LOAD(idMajor, idMinor, idSub);
        _logger.WriteLine($"NPC Handle: {npcHandle}");
        _flowCaller.FLD_MODEL_LOADSYNC(npcHandle);
        _flowCaller.FLD_MODEL_SET_TRANSLATE(npcHandle,x,y,z,0);
        _flowCaller.FLD_MODEL_SET_ROTATE(npcHandle,xr,yr,zr,0);
        _flowCaller.FLD_MODEL_SET_VISIBLE(npcHandle,1,0);
    }

    public void ShowJoker(int show)
    {
        int joker = _flowCaller.FLD_PC_GET_RESHND(0);
        _flowCaller.FLD_MODEL_SET_VISIBLE(joker,show,0);
    }
}