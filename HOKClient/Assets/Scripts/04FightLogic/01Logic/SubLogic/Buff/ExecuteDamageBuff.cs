/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 百分比生命值斩杀

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class ExecuteDamageBuffCfg : BuffCfg {
    public int damagePct;
}

public class ExecuteDamageBuff : Buff {
    PEInt damagePct;

    public ExecuteDamageBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        ExecuteDamageBuffCfg edbc = cfg as ExecuteDamageBuffCfg;
        damagePct = edbc.damagePct;
    }

    protected override void Start() {
        base.Start();

        PEInt damage = (damagePct / 100) * owner.ud.unitCfg.hp;
        owner.GetDamageByBuff(damage, this);
    }
}
