/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 子弹命中动态晕眩时间Buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class StunBuffCfg_DynamicTime : BuffCfg {
    public int minStunTime;
    public int maxStunTime;
}

public class StunBuff_DynamicTime : Buff {
    public StunBuff_DynamicTime(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        StunBuffCfg_DynamicTime dtsbc = cfg as StunBuffCfg_DynamicTime;
        int argsTime = (int)args[0];
        if(argsTime < dtsbc.minStunTime) {
            argsTime = dtsbc.minStunTime;
        }
        if(argsTime > dtsbc.maxStunTime) {
            argsTime = dtsbc.maxStunTime;
        }
        buffDuration = argsTime;
    }

    protected override void Start() {
        base.Start();

        owner.StunnedCount += 1;
    }

    protected override void End() {
        base.End();

        owner.StunnedCount -= 1;
    }
}
