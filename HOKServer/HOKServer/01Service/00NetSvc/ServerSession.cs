/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 18:05
	功能: .net core 服务端Session连接

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PENet;
using System;
using HOKProtocol;

namespace HOKServer {
    public class ServerSession : KCPSession<HOKMsg> {
        protected override void OnConnected() {
            this.ColorLog(PEUtils.LogColor.Green, "Client Online,Sid:{0}", m_sid);
        }

        protected override void OnDisConnected() {
            this.Warn("Client Offlien,Sid:{0}", m_sid);
        }

        protected override void OnReciveMsg(HOKMsg msg) {
            this.Log("RcvPack CMD:{0}", msg.cmd.ToString());
            NetSvc.Instance.AddMsgQue(this, msg);
        }

        protected override void OnUpdate(DateTime now) {
        }
    }
}
