/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: Buff逻辑

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System.Collections.Generic;
using UnityEngine;


public class Buff : SubLogicUnit {
    /// <summary>
    /// buff附着单位
    /// </summary>
    public MainLogicUnit owner;
    protected int buffID;
    protected object[] args;

    protected int buffDuration;
    int tickCount = 0;//Dot计数
    int durationCount = 0;//时长计时
    public BuffCfg cfg;

    /// <summary>
    /// 群体buff作用目标列表
    /// </summary>
    protected List<MainLogicUnit> targetLst;
    //表现数据
    BuffView buffView;

    public Buff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, skill) {
        this.owner = owner;
        this.buffID = buffID;
        this.args = args;
    }

    public override void LogicInit() {
        cfg = ResSvc.Instance.GetBuffConfigByID(buffID);
        buffDuration = cfg.buffDuration;
        delayTime = cfg.buffDelay;

        base.LogicInit();
    }

    public override void LogicTick() {
        base.LogicTick();
        switch(unitState) {
            case SubUnitState.Start:
                Start();
                if(buffDuration > 0 || buffDuration == -1) {
                    unitState = SubUnitState.Tick;
                }
                else {
                    unitState = SubUnitState.End;
                }
                break;
            case SubUnitState.Tick:
                if(cfg.buffInterval > 0) {
                    tickCount += ServerConfig.ServerLogicFrameIntervelMs;
                    if(tickCount >= cfg.buffInterval) {
                        tickCount -= cfg.buffInterval;
                        Tick();
                    }
                }
                durationCount += ServerConfig.ServerLogicFrameIntervelMs;
                if(durationCount >= buffDuration && buffDuration != -1) {
                    unitState = SubUnitState.End;
                }
                break;
        }
    }

    protected override void Start() {
        //根据staticPosType决定Buff初始位置
        if(cfg.staticPosType == StaticPosTypeEnum.None) {
            LogicPos = owner.LogicPos;
        }

        if(cfg.buffEffect != null) {
            GameObject go = ResSvc.Instance.LoadPrefab("ResEffects/" + cfg.buffEffect);
            go.name = source.unitName + "_" + cfg.buffName;
            buffView = go.GetComponent<BuffView>();
            if(buffView == null) {
                this.Error("Get BuffView Error:" + unitName);
            }

            if(cfg.staticPosType == StaticPosTypeEnum.None) {
                owner.mainViewUnit.SetBuffFollower(buffView);
            }
            buffView.Init(this);

            if(cfg.buffAudio != null) {
                buffView.PlayAudio(cfg.buffAudio);
            }
        }
        else {
            if(cfg.buffAudio != null) {
                owner.PlayAudio(cfg.buffAudio);
            }
        }
    }
    protected override void Tick() {
        if(cfg.hitTickAudio != null && targetLst.Count > 0) {
            owner.PlayAudio(cfg.hitTickAudio);
        }
    }
    protected override void End() {
        if(cfg.buffEffect != null) {
            buffView.DestroyBuff();
        }
    }
}
