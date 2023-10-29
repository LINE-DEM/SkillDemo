/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 后羿1技能修改buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/
public class HouyiScatterSkillModifyBuffCfg : BuffCfg {
    public int originalID;
    public int powerID;
    public int superPowerID;
}


public class HouyiScatterSkillModifyBuff : Buff {
    int originalID;
    int powerID;
    int superPowerID;
    Skill modifySkill;

    public HouyiScatterSkillModifyBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        HouyiScatterSkillModifyBuffCfg hssmbc = cfg as HouyiScatterSkillModifyBuffCfg;
        originalID = hssmbc.originalID;
        powerID = hssmbc.powerID;
        superPowerID = hssmbc.superPowerID;
        modifySkill = owner.GetSkillByID(originalID);
    }

    protected override void Start() {
        base.Start();

        if(modifySkill.TempSkillID == 0) {
            modifySkill.ReplaceSkillCfg(powerID);
        }
        else {
            modifySkill.ReplaceSkillCfg(superPowerID);
        }
    }

    protected override void End() {
        base.End();
        if(modifySkill.TempSkillID == powerID) {
            modifySkill.ReplaceSkillCfg(originalID);
        }
        else {
            modifySkill.ReplaceSkillCfg(1025);
        }
    }
}
