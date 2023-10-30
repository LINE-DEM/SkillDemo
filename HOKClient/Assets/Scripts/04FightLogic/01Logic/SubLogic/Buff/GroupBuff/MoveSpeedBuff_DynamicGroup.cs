/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 动态查找目标修改移速

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using System.Collections.Generic;

public class MoveSpeedBuff_DynamicGroup : Buff {
    PEInt speedOffset;

    public MoveSpeedBuff_DynamicGroup(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();
        targetLst = new List<MainLogicUnit>();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, skill.skillArgs));
        MoveSpeedBuffCfg msbc = cfg as MoveSpeedBuffCfg;
        speedOffset = msbc.amount;
    }

    protected override void Start() {
        base.Start();

        ModifyMoveSpeed(speedOffset, true);
    }

    protected override void Tick() {
        base.Tick();
        ModifyMoveSpeed(-speedOffset);

        targetLst.Clear();
        targetLst.AddRange(CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, PEVector3.zero));
        ModifyMoveSpeed(speedOffset);
    }

    protected override void End() {
        base.End();
        ModifyMoveSpeed(-speedOffset);
        targetLst.Clear();
        targetLst = null;
    }

    void ModifyMoveSpeed(PEInt value, bool showJump = false) {
        for(int i = 0; i < targetLst.Count; i++) {
            PEInt offset = targetLst[i].moveSpeedBase * (value / 100);
            targetLst[i].ModifyMoveSpeed(offset, this, showJump);
        }
    }
}
