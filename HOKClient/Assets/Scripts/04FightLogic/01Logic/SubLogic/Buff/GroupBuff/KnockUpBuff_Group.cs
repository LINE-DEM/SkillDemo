/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 范围击飞Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class KnockUpBuff_Group : Buff {
    public KnockUpBuff_Group(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    protected override void Start() {
        base.Start();

        targetLst = new System.Collections.Generic.List<MainLogicUnit>();
        targetLst = CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, skill.skillArgs);
        for(int i = 0; i < targetLst.Count; i++) {
            targetLst[i].KnockupCount += 1;
        }
    }

    protected override void End() {
        base.End();

        for(int i = 0; i < targetLst.Count; i++) {
            targetLst[i].KnockupCount -= 1;
        }
    }
}
