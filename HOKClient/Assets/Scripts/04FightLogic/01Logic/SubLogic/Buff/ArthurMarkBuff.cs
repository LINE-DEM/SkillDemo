/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 亚瑟1技能标记Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class ArthurMarkBuffCfg : BuffCfg {
    public int damagePct;
}

public class ArthurMarkBuff : Buff {
    PEInt damagePct;
    MainLogicUnit target;

    public ArthurMarkBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        ArthurMarkBuffCfg ambc = cfg as ArthurMarkBuffCfg;
        damagePct = ambc.damagePct;
        target = skill.lockTarget;
    }

    protected override void Start() {
        base.Start();
        target.OnHurt += GetHurt;
    }

    void GetHurt() {
        target.GetDamageByBuff(damagePct / 100 * target.ud.unitCfg.hp, this, false);
    }

    protected override void End() {
        base.End();
        target.OnHurt -= GetHurt;
    }
}
