/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 单体移速buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class MoveSpeedBuffCfg : BuffCfg {
    public int amount;//速度改变量，百分比
}

public class MoveSpeedBuff_Single : Buff {
    private PEInt speedOffset;

    public MoveSpeedBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        MoveSpeedBuffCfg msbc = cfg as MoveSpeedBuffCfg;
        speedOffset = owner.moveSpeedBase * ((PEInt)msbc.amount / 100);
    }

    protected override void Start() {
        base.Start();
        owner.ModifyMoveSpeed(speedOffset, this, true);
    }

    protected override void End() {
        base.End();
        owner.ModifyMoveSpeed(-speedOffset, this, false);
    }
}
