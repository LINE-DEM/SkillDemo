/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 16:01
	功能: 服务器启动入口

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using System.Threading;

namespace HOKServer {
    class ServerStart {
        static void Main(string[] args) {
            ServerRoot.Instance.Init();

            while(true) {
                ServerRoot.Instance.Update();
                Thread.Sleep(10);
            }
        }
    }
}
