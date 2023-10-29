/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 主要逻辑单位移动碰撞处理

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
using System.Collections.Generic;

public partial class MainLogicUnit {

    /// <summary>
    /// UI输入方向
    /// </summary>
    private PEVector3 inputDir;
    public PEVector3 InputDir {
        private set {
            inputDir = value;
        }
        get {
            return inputDir;
        }
    }

    /// <summary>
    /// 战斗单位物理碰撞器
    /// </summary>
    public PECylinderCollider collider;
    List<PEColliderBase> envColliLst;
    void InitMove() {
        LogicPos = ud.bornPos;
        moveSpeedBase = ud.unitCfg.moveSpeed;
        LogicMoveSpeed = ud.unitCfg.moveSpeed;
        envColliLst = BattleSys.Instance.GetEnvColliders();
        collider = new PECylinderCollider(ud.unitCfg.colliCfg) {
            mPos = LogicPos
        };
    }

    void TickMove() {
        PEVector3 moveDir = InputDir;
        collider.mPos += moveDir * LogicMoveSpeed * (PEInt)Configs.ClientLogicFrameDeltaSec;
        PEVector3 adj = PEVector3.zero;
        collider.CalcCollidersInteraction(envColliLst, ref moveDir, ref adj);
        if(LogicDir != moveDir) {
            LogicDir = moveDir;
        }
        if(LogicDir != PEVector3.zero) {
            LogicPos = collider.mPos + adj;
        }
        collider.mPos = LogicPos;
        //this.Log($"{unitName} pos:" + collider.mPos.ConvertViewVector3());
    }

    void UnInitMove() {
        //TODO
    }

    /// <summary>
    /// 临时存储UI输入方向，以便于施放技能再恢复方向输入
    /// </summary>
    private PEVector3 UIInputDir;
    public void InputMoveKey(PEVector3 dir) {
        UIInputDir = dir;
        if(IsSkillSpelling() == false
            && IsStunned() == false
            && IsKnockup() == false) {
            InputDir = dir;
        }
    }

    /// <summary>
    /// 模拟输入方向
    /// </summary>
    /// <param name="dir"></param>
    public void InputFakeMoveKey(PEVector3 dir) {
        InputDir = dir;
    }

    public void RecoverUIInput() {
        if(InputDir != UIInputDir) {
            InputDir = UIInputDir;
        }
    }

    /// <summary>
    /// 是否能移动
    /// 未被晕眩/击飞
    /// 未在其它技能施法前摇阶段
    /// </summary>
    /// <returns></returns>
    public bool CanMove() {
        return IsStunned() == false
            && IsKnockup() == false
            && IsSkillSpelling() == false;
    }
}
