/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 18:46
	功能: 客户端网络连接会话

    //=================*=================\\
           教学官网：www.qiqiker.com
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PENet;
using System;

public class ClientSession : KCPSession<HOKMsg> {
    protected override void OnConnected() {
        GameRoot.Instance.ShowTips("连接服务器成功");
    }

    protected override void OnDisConnected() {
        GameRoot.Instance.ShowTips("断开服务器连接");
    }

    protected override void OnReciveMsg(HOKMsg msg) {
        this.ColorLog(PEUtils.LogColor.Green, "RcvCMD:" + msg.cmd.ToString());
        NetSvc.Instance.AddMsgQue(msg);
    }

    protected override void OnUpdate(DateTime now) {
    }
}
