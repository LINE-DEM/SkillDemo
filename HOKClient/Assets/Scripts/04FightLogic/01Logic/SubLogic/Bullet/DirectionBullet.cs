/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 方向指向子弹

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;
using PEPhysx;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DirectionBullet : Bullet {
    //定点位置
    PEVector3 targetPos;
    PECylinderCollider targetCollider;

    //定向飞行计时
    int bulletTime;

    //是否停留
    private bool stay;
    public Action<MainLogicUnit, object[]> hitTargetCB;
    public Action<object[]> ReachPosCB;
    public Action<object[]> EndCB;

    public DirectionBullet(MainLogicUnit source, Skill skill)
        : base(source, skill) {
    }

    public override void LogicInit() {
        base.LogicInit();

        BulletTypeEnum bte = cfg.bulletType;
        //指定方向飞行的子弹
        if(bte == BulletTypeEnum.UIDirection) {
            if(skill.skillArgs == PEVector3.zero) {
                this.Error("input skill direction is vector2.zero.");
            }
            else {
                LogicDir = skill.skillArgs;
            }
        }
        //指定位置飞行的子弹
        else if (bte == BulletTypeEnum.UIPosition || bte == BulletTypeEnum.UIDirectionNoCollider)
        {
            if (stay)
            {
                
            }
            targetPos = source.LogicPos + skill.skillArgs + new PEVector3(0, (PEInt)cfg.bulletHeight, 0);
            ColliderConfig targetColliderCfg = new ColliderConfig
            {
                mPos = targetPos,
                mType = ColliderType.Cylinder,
                mRadius = bulletSize
            };
            targetCollider = new PECylinderCollider(targetColliderCfg);

            LogicDir = (targetPos - LogicPos).normalized;
            
        }
        else {
            this.Log("Unknow Bullet Type Enum.");
        }

        LogicPos += LogicDir * (PEInt)cfg.bulletOffset;
    }

    /*
    int tickCount = 0;
    GameObject ghostRoot;
    */
    protected override void Start() {
        base.Start();

        #region Debug 显示
        /*
        ghostRoot = new GameObject {
            name = "弹道GhostRoot"
        };
        ghostRoot.transform.localPosition = Vector3.zero;
        UnityEngine.Object.Destroy(ghostRoot, 5);
        */
        #endregion
    }

    protected override void Tick() {
        base.Tick();

        if (!stay)
        {
            LogicPos += LogicDir * LogicMoveSpeed;
        }

        //SweepVolume算法
        PEVector3 pos = (LogicPos + lastPos) / 2;
        PEVector3 offset = LogicPos - lastPos;
        ColliderConfig volumeCfg = new ColliderConfig {
            mType = ColliderType.Box,
            mPos = pos,
            mSize = new PEVector3 {
                x = offset.magnitude / 2,
                y = 0,
                z = bulletSize
            },
            mAxis = new PEVector3[3]
        };
        volumeCfg.mAxis[0] = offset.normalized;
        volumeCfg.mAxis[1] = PEVector3.up;
        volumeCfg.mAxis[2] = PEVector3.Cross(offset, PEVector3.up).normalized;
        PEBoxCollider volumeCollider = new PEBoxCollider(volumeCfg);
        lastPos = LogicPos;
        if (!cfg.isNoDamage)
        {
            
            #region 弹道显示 
            /*
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.SetParent(ghostRoot.transform);
            tickCount += 1;
            go.name = "ghost_" + tickCount;
            go.GetComponent<MeshRenderer>().enabled = false;
            go.transform.position = volumeCfg.mPos.ConvertViewVector3();
            go.transform.right = volumeCfg.mAxis[0].ConvertViewVector3();
            go.transform.up = volumeCfg.mAxis[1].ConvertViewVector3();
            go.transform.forward = volumeCfg.mAxis[2].ConvertViewVector3();
            go.transform.localScale = volumeCfg.mSize.ConvertViewVector3() * 2;
            */
            #endregion

            List<MainLogicUnit> hitLst = new List<MainLogicUnit>();
            List<MainLogicUnit> selectLst = CalcRule.FindMulipleTargetByRule(source, cfg.impacter, PEVector3.zero);
            for(int i = 0; i < selectLst.Count; i++) {
                PEVector3 normal = PEVector3.zero;
                PEVector3 adj = PEVector3.zero;
                if(selectLst[i].collider.DetectContact(volumeCollider, ref normal, ref adj)) {
                    hitLst.Add(selectLst[i]);
                }
            }

            if(cfg.canBlock) {
                //可被阻挡，只作用与上一个位置最近的目标，eg:houyi 大招
                if(hitLst.Count > 0) {
                    MainLogicUnit hitTarget = CalcRule.FindMinDisTargetInPos(lastPos, hitLst.ToArray());
                    hitTargetCB(hitTarget, new object[] { bulletTime, hitTarget.LogicPos });
                    unitState = SubUnitState.End;
                }
            }
            else {
                //不可阻挡，穿透所有目标，飞到目标位置停止
                for(int i = 0; i < hitLst.Count; i++) {
                    hitTargetCB(hitLst[i], new object[] { bulletTime, hitLst[i].LogicPos });
                }
            }
        
        }
            
            
        

        //是否达到目标位置
        if(cfg.bulletType == BulletTypeEnum.UIPosition) {
            PEVector3 normal = PEVector3.zero;
            PEVector3 adj = PEVector3.zero;
            if(targetCollider.DetectContact(volumeCollider, ref normal, ref adj)) {
                unitState = SubUnitState.End;
            }
        }
        else if(cfg.bulletType == BulletTypeEnum.UIDirection) {
            bulletTime += ServerConfig.ServerLogicFrameIntervelMs;
            if(bulletTime >= cfg.bulletDuration) {
                unitState = SubUnitState.End;
            }
        }
        //先到达位置  然后计时
        else if(cfg.bulletType == BulletTypeEnum.UIDirectionNoCollider) {
            PEVector3 distanceVector3 = targetPos - LogicPos;
            PEInt distance = distanceVector3.x + distanceVector3.y + distanceVector3.z;
            if (distance < 1)
            {
                stay = true;
                bulletTime += ServerConfig.ServerLogicFrameIntervelMs;
                if(bulletTime >= cfg.bulletDuration) {
                    unitState = SubUnitState.End;
                }
            }
        }

        lastPos = LogicPos;
    }

    protected override void End() {
        base.End();
        
        EndCB(new object[] { bulletTime, targetPos });
    }
}
