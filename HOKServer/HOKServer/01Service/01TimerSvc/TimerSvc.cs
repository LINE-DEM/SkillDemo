/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 16:04
	功能: 定时服务

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using PETimer;

namespace HOKServer {
    public class TimerSvc : Singleton<TimerSvc> {
        TickTimer timer = new TickTimer(0, false);
        public override void Init() {
            base.Init();

            timer.LogFunc = this.Log;
            timer.WarnFunc = this.Warn;
            timer.ErrorFunc = this.Error;

            this.Log("TimeSvc Init Done.");
        }

        public override void Update() {
            base.Update();

            timer.UpdateTask();
        }

        public int AddTask(uint delay, Action<int> taskCB, Action<int> cancelCB = null, int count = 1) {
            return timer.AddTask(delay, taskCB, cancelCB, count);
        }

        public bool DeleteTask(int tid) {
            return timer.DeleteTask(tid);
        }
    }
}
