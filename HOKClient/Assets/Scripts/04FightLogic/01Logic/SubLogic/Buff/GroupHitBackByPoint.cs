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
public class GroupHitBackByPointCfg : BuffCfg {
    public float speed;         //位移速度
    public float distance;
}

public class GroupHitBackByPoint : Buff {
    PEInt speed;
    PEInt distance;
    private PEVector3 skillPoint;
    private PEVector3 tempPoint;
    private List<MainLogicUnit> hitList;
    private List<PEVector3> dirList = new List<PEVector3>();
   
    public GroupHitBackByPoint(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
        
    }

    public override void LogicInit() {
        base.LogicInit();
        GroupHitBackByPointCfg tfmbc = cfg as GroupHitBackByPointCfg;
        speed = (PEInt)tfmbc.speed;
        distance = (PEInt)tfmbc.distance;
        
        skillPoint= skill.skillArgs + owner.LogicPos;
        hitList = CalcRule.FindMulipleTargetByRule(owner, cfg.impacter, skillPoint);
        foreach (var hero in hitList)
        {
            dirList.Add((   hero.LogicPos - skillPoint).normalized); 
        }
       
    }

    protected override void Start() {
        base.Start();
        
    }

    public override void LogicTick()
    {
        base.LogicTick();
        
        for (int i = 0; i < hitList.Count; i++)
        {
            hitList[i].InputFakeMoveKey(PEVector3.zero);

            tempPoint = hitList[i].LogicPos + (dirList[i] * distance);
                
            hitList[i].LogicPos += dirList[i] * speed * (PEInt)Configs.ClientLogicFrameDeltaSec;

            if ((hitList[i].LogicPos - tempPoint).magnitude <=(PEInt)1)
            {
                unitState = SubUnitState.None;
            }
        }
        
        
       
        
    }
}