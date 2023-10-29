/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 目标闪现移动buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class TargetFlashMoveBuffCfg : BuffCfg {
    public float offset;//目标偏移量
}

public class TargetFlashMoveBuff : Buff {
    PEInt offset;

    public TargetFlashMoveBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();
        TargetFlashMoveBuffCfg tfmbc = cfg as TargetFlashMoveBuffCfg;
        offset = (PEInt)tfmbc.offset;
    }

    protected override void Start() {
        base.Start();

        MainLogicUnit target = CalcRule.FindSingleTargetByRule(owner, skill.cfg.targetCfg, PEVector3.zero);
        if(target == null) {
            unitState = SubUnitState.End;
        }
        else {
            PEVector3 disVec = target.LogicPos - owner.LogicPos;
            owner.LogicPos += disVec.normalized * (disVec.magnitude - offset);
        }
    }
}