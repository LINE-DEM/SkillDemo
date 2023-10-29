/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 范围伤害Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using System.Collections.Generic;

public class DamageBuffCfg_DynamicGroup : BuffCfg {
    /// <summary>
    /// 每次Tick伤害
    /// </summary>
    public int damage;
}

public class DamageBuff_DynamicGroup : Buff {
    PEInt damage;

    public DamageBuff_DynamicGroup(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        targetLst = new List<MainLogicUnit>();
        DamageBuffCfg_DynamicGroup gdbc = cfg as DamageBuffCfg_DynamicGroup;
        damage = gdbc.damage;
    }

    protected override void Tick() {
        base.Tick();
        CalcGroupDamage();
    }

    void CalcGroupDamage() {
        targetLst.Clear();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, PEVector3.zero));
        for(int i = 0; i < targetLst.Count; i++) {
            targetLst[i].GetDamageByBuff(damage, this);
        }
    }
}
