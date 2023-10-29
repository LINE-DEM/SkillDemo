/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 逻辑定时器

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;
using System;

public class LogicTimer {
    bool isActive;
    public bool IsActive {
        private set {
            isActive = value;
        }
        get {
            return isActive;
        }
    }

    PEInt delayTime;
    PEInt loopTime;

    PEInt delta;
    PEInt callbackCount;
    Action cb;

    public LogicTimer(Action cb, PEInt delayTime, int loopTime = 0) {
        this.cb = cb;
        this.delayTime = delayTime;
        this.loopTime = loopTime;
        delta = ServerConfig.ServerLogicFrameIntervelMs;
        IsActive = true;
    }

    public void TickTimer() {
        callbackCount += delta;
        if(callbackCount >= delayTime && cb != null) {
            cb();
            if(loopTime == 0) {
                IsActive = false;
                cb = null;
            }
            else {
                callbackCount -= delayTime;
                delayTime = loopTime;
            }
        }
    }
}
