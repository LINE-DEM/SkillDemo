/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 血量回复Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class HPCureBuffCfg : BuffCfg {
    public int cureHPpct;
}

public class HPCureBuff_Single : Buff {
    public PEInt cureHPpct;

    public HPCureBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        HPCureBuffCfg hcbc = cfg as HPCureBuffCfg;
        cureHPpct = hcbc.cureHPpct;
    }

    protected override void Tick() {
        base.Tick();
        if(owner.unitState == UnitStateEnum.Alive) {
            owner.GetCureByBuff(owner.ud.unitCfg.hp * cureHPpct / 100, this);
        }
    }
}
