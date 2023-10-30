/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 18:06
	功能: 通用配置数据

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

namespace HOKProtocol {
    public class ServerConfig {

        //确认匹配倒计时：15秒
        public const int ConfirmCountDown = 15;
        //选择英雄倒计时：15秒
        public const int SelectCountDown = 15;


        public const string LocalDevInnerIP = "192.168.1.100";
        public const int UdpPort = 17666;
        public const int ServerLogicFrameIntervelMs = 66;
    }

    public class Configs {
        public const float ClientLogicFrameDeltaSec = 0.066f;//s
    }
}
