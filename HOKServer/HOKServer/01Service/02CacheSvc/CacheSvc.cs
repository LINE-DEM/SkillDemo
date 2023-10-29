/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 16:05
	功能: 缓存服务

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer {
    public class CacheSvc : Singleton<CacheSvc> {
        //acct-session
        private Dictionary<string, ServerSession> onLineAcctDic;
        //seesion-userdata
        private Dictionary<ServerSession, UserData> onLineSessionDic;

        public override void Init() {
            base.Init();
            onLineAcctDic = new Dictionary<string, ServerSession>();
            onLineSessionDic = new Dictionary<ServerSession, UserData>();

            this.Log("CacheSvc Init Done.");
        }

        public override void Update() {
            base.Update();
        }

        public bool IsAcctOnLine(string acct) {
            return onLineAcctDic.ContainsKey(acct);
        }

        public void AcctOnline(string acct, ServerSession session, UserData playerData) {
            onLineAcctDic.Add(acct, session);
            onLineSessionDic.Add(session, playerData);
        }

        public UserData GetUserDataBySession(ServerSession session) {
            if(onLineSessionDic.TryGetValue(session, out UserData playerData)) {
                return playerData;
            }
            else {
                return null;
            }
        }
    }
}
