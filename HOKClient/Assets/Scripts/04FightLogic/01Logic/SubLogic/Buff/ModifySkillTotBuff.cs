/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 技能替换Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class ModifySkillTotBuffCfg : BuffCfg {
    public int originalID;
    public int replaceID;
}

public class ModifySkillTotBuff : Buff {
    public int originalID;
    public int replaceID;
    private Skill modifySkill;

    public ModifySkillTotBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        ModifySkillTotBuffCfg mabc = cfg as ModifySkillTotBuffCfg;
        originalID = mabc.originalID;
        replaceID = mabc.replaceID;
        owner.SetSkillByID(originalID, replaceID);
        
        //如果替换技能 还有冷却时间 就显示这个冷却时间
        int cdTime = owner.GetSkillByID(replaceID).realesTime;
        if (cdTime>0)
        {
            BattleSys.Instance.EnterCDState(replaceID, cdTime*1000);
        }
        
        //modifySkill = owner.GetSkillByID(originalID);
    }

    // protected override void Start() {
    //     base.Start();
    //
    //     modifySkill.ReplaceSkillCfg(replaceID);
    //     modifySkill.SpellSuccCallback += ReplaceSkillReleaseDone;
    // }
    //
    // void ReplaceSkillReleaseDone(Skill skillReleased) {
    //     if(skillReleased.cfg.isNormalAttack) {
    //         unitState = SubUnitState.End;
    //     }
    // }
    //
    // protected override void End() {
    //     base.End();
    //     modifySkill.ReplaceSkillCfg(originalID);
    //     modifySkill.SpellSuccCallback -= ReplaceSkillReleaseDone;
    // }
}
