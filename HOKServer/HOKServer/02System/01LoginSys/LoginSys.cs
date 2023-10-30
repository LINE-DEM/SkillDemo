/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/26 13:37
	功能: 登录业务系统

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
    public class LoginSys : SystemRoot<LoginSys> {
        public override void Init() {
            base.Init();

            this.Log("LoginSys Init Done.");
        }

        public override void Update() {
            base.Update();
        }

        public void ReqLogin(MsgPack pack) {
            ReqLogin data = pack.msg.reqLogin;

            HOKMsg msg = new HOKMsg {
                cmd = CMD.RspLogin
            };

            if(cacheSvc.IsAcctOnLine(data.acct)) {
                //已上线，返回错误信息
                msg.error = ErrorCode.AcctIsOnline;
            }
            else {
                //未上线，无缓存，创建默认账号数据，并缓存
                uint sid = pack.session.GetSessionID();
                UserData ud = new UserData {
                    id = sid,
                    name = "Plane_" + sid,
                    lv = 17,
                    exp = 10086,
                    coin = 999,
                    diamond = 666,
                    ticket = 0,
                    heroSelectData = new List<HeroSelectData> {
                        new HeroSelectData {
                            heroID = 101,
                        },
                        new HeroSelectData {
                            heroID =102
                        }
                    }
                };

                msg.rspLogin = new RspLogin {
                    userData = ud
                };
                cacheSvc.AcctOnline(data.acct, pack.session, ud);
            }
            pack.session.SendMsg(msg);
        }
    }
}
