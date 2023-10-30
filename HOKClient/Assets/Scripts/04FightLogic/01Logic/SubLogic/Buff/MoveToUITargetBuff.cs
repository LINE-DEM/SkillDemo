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

using System;
using PEMath;
using HOKProtocol;
using System.Collections.Generic;
public class MoveToUITargetBuffCfg : BuffCfg {
    public float speed;//位移速度
    
}

public class MoveToUITargetBuff : Buff {
    PEInt speed;
    private PEVector3 disVec;
    public Action<List<MainLogicUnit>> TouchEnmycb;
    public MoveToUITargetBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
        disVec= skill.skillArgs - owner.LogicPos;
    }

    public override void LogicInit() {
        base.LogicInit();
        
        MoveToUITargetBuffCfg tfmbc = cfg as MoveToUITargetBuffCfg;
        speed = (PEInt)tfmbc.speed;
    }

    protected override void Start() {
        base.Start();
        
    }

    public override void LogicTick()
    {
        base.LogicTick();
        owner.LogicPos += disVec.normalized * speed * (PEInt)Configs.ClientLogicFrameDeltaSec;
        if ((owner.LogicPos - skill.skillArgs).magnitude <=(PEInt)0.1)
        {
            unitState = SubUnitState.End;
        }
        
        //这里的碰撞检测逻辑最好分开写 返回
        List<MainLogicUnit> hitLst = new List<MainLogicUnit>();
        List<MainLogicUnit> selectLst = CalcRule.FindMulipleTargetByRule(source, cfg.impacter, PEVector3.zero);
        for(int i = 0; i < selectLst.Count; i++) {
            PEVector3 normal = PEVector3.zero;
            PEVector3 adj = PEVector3.zero;
            if(selectLst[i].collider.DetectContact(owner.collider, ref normal, ref adj)) {
                hitLst.Add(selectLst[i]);
            }
        }
        
        //如果检测到敌人 抛出事件
        if(hitLst.Count > 0)
        {
            TouchEnmycb?.Invoke(hitLst);
            
            unitState = SubUnitState.End;
            // MainLogicUnit hitTarget = CalcRule.FindMinDisTargetInPos(lastPos, hitLst.ToArray());
            // hitTargetCB(hitTarget, new object[] { bulletTime, hitTarget.LogicPos });
            // unitState = SubUnitState.End;
        }
    }
}