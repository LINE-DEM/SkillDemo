/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 基础逻辑单位

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using System;

public abstract class LogicUnit : ILogic {
    /// <summary>
    /// 产生UI输入时
    /// </summary>
    public Action OnDirChange;
    /// <summary>
    /// 逻辑单位角色名称
    /// </summary>
    public string unitName;

    #region Key Properties
    //逻辑位置
    public bool isPosChanged = false;
    PEVector3 logicPos;
    public PEVector3 LogicPos {
        set {
            logicPos = value;
            isPosChanged = true;
        }
        get {
            return logicPos;
        }
    }
    //逻辑方向
    public bool isDirChanged = false;
    PEVector3 logicDir;
    public PEVector3 LogicDir {
        set {
            logicDir = value;
            isDirChanged = true;
            OnDirChange?.Invoke();
        }
        get {
            return logicDir;
        }
    }
    //逻辑速度
    PEInt logicMoveSpeed;
    public PEInt LogicMoveSpeed {
        set {
            logicMoveSpeed = value;
        }
        get {
            return logicMoveSpeed;
        }
    }
    /// <summary>
    /// 基础速度
    /// </summary>
    public PEInt moveSpeedBase;
    #endregion

    public abstract void LogicInit();
    public abstract void LogicTick();
    public abstract void LogicUnInit();
}

interface ILogic {
    void LogicInit();
    void LogicTick();
    void LogicUnInit();
}
