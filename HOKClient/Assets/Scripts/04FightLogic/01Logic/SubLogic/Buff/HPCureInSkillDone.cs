/*************************************************
    作者: Plane
    邮箱: 1785275942@qq.com
    功能: 只要释放技能就血量回复Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;

public class HPCureInSkillDone_cfg : BuffCfg {
    public int cureHPpct;
    public int buffCd;
}

public class HPCureInSkillDone : Buff {
    public PEInt cureHPpct;
    public int buffCd;
    public int cdNow = 0;
    public Skill[] skills;
    public HPCureInSkillDone(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        HPCureInSkillDone_cfg hcbc = cfg as HPCureInSkillDone_cfg;
        cureHPpct = hcbc.cureHPpct;
        buffCd= hcbc.buffCd;
        skills = owner.GetAllSkill();
        foreach (var skill in skills)
        {
            skill.SpellSuccCallback += OnSpellSkillSucc;
        }
    }

    public void OnSpellSkillSucc(Skill skillReleased)
    {
        if(owner.unitState == UnitStateEnum.Alive && cdNow <= 0)
        {
            cdNow = buffCd*1000;
            owner.GetCureByBuff(owner.ud.unitCfg.hp * cureHPpct / 100, this);
        }
    }
    
    int timeCount;
    bool inCd = true;
    protected override void Tick() {
        base.Tick();
        if(inCd) {
            cdNow -= ServerConfig.ServerLogicFrameIntervelMs;
            if(cdNow >= 0)
            {
                inCd = false;
            }
        }
    }
}
