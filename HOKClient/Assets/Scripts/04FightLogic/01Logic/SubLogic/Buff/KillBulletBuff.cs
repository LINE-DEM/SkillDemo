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

public class KillBulletBuffCfg : BuffCfg
{
    public int skillId;
}

public class KillBulletBuff : Buff {
    

    public KillBulletBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        KillBulletBuffCfg mabc = cfg as KillBulletBuffCfg;
        
        
        owner.GetSkillByID(mabc.skillId).bullet.unitState = SubUnitState.End;
        
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
