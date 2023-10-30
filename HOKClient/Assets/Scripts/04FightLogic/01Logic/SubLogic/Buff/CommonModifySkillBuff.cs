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

public class CommonModifySkillBuffCfg : BuffCfg {
    public int originalID;
    public int replaceID;
}

public class CommonModifySkillBuff : Buff {
    public int originalID;
    public int replaceID;
    private Skill modifySkill;

    public CommonModifySkillBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        CommonModifySkillBuffCfg mabc = cfg as CommonModifySkillBuffCfg;
        originalID = mabc.originalID;
        replaceID = mabc.replaceID;
        modifySkill = owner.GetSkillByID(originalID);
    }

    protected override void Start() {
        base.Start();

        modifySkill.ReplaceSkillCfg(replaceID);
        modifySkill.SpellSuccCallback += ReplaceSkillReleaseDone;
    }

    void ReplaceSkillReleaseDone(Skill skillReleased) {
        if(skillReleased.cfg.isNormalAttack) {
            unitState = SubUnitState.End;
        }
    }

    protected override void End() {
        base.End();
        modifySkill.ReplaceSkillCfg(originalID);
        modifySkill.SpellSuccCallback -= ReplaceSkillReleaseDone;
    }
}
