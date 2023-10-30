/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 辅助逻辑单位

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;

public abstract class SubLogicUnit : LogicUnit {
    /// <summary>
    /// 辅助单元来源角色
    /// </summary>
    public MainLogicUnit source;
    /// <summary>
    /// 辅助单元所属技能
    /// </summary>
    protected Skill skill;
    /// <summary>
    /// 延迟生效时间
    /// </summary>
    protected int delayTime;
    /// <summary>
    /// 延迟时间计数
    /// </summary>
    protected int delayCounter;
    /// <summary>
    /// 辅助单元状态
    /// </summary>
    public SubUnitState unitState;

    public SubLogicUnit(MainLogicUnit source, Skill skill) {
        this.source = source;
        this.skill = skill;
    }

    public override void LogicInit() {
        if(delayTime == 0) {
            unitState = SubUnitState.Start;
        }
        else {
            delayCounter = delayTime;
            unitState = SubUnitState.Delay;
        }
    }

    public override void LogicTick() {
        switch(unitState) {
            case SubUnitState.Delay:
                delayCounter -= ServerConfig.ServerLogicFrameIntervelMs;
                if(delayCounter <= 0) {
                    unitState = SubUnitState.Start;
                }
                break;
            case SubUnitState.End:
                End();
                unitState = SubUnitState.None;
                break;
            case SubUnitState.None:
            default:
                break;
        }
    }

    public override void LogicUnInit() { }

    protected abstract void Start();
    protected abstract void Tick();
    protected abstract void End();
}

public enum SubUnitState {
    None,
    Delay,
    Start,
    Tick,
    End
}
