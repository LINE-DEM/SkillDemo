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
public class jiutongSkillTwoCfg : BuffCfg {
    
    
}

public class jiutongSkillTwo : Buff
{
    private bool isFirstHit = true;
    public jiutongSkillTwo(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
        
    }

    public override void LogicInit() {
        base.LogicInit();
        Buff needBuff = owner.GetBuffByID(10305);
        if (needBuff!=null)
        {
            MoveToUITargetBuff moveBuff = needBuff as MoveToUITargetBuff;
            moveBuff.TouchEnmycb += TouchEnmy;
        }
        
    }
    

    //碰撞到敌人触发
    public void TouchEnmy(List<MainLogicUnit> Units)
    {
        if (isFirstHit)
        {
            isFirstHit = false;
            //范围眩晕buff ExecuteDamageBuffCfg
            owner.CreateSkillBuff(Units[0], skill, 10307);
            //关闭强制位移
            owner.GetBuffByID(10305).unitState = SubUnitState.None;
        }
        
        
    }
    public override void LogicTick()
    {
        
    }
}