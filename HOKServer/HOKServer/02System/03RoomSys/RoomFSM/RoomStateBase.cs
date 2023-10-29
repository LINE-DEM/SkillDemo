/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/06 2:04
	功能: 房间状态抽象基类

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信服务号: qiqikertuts
           关注微信公众号: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

namespace HOKServer {
    public interface IRoomState {
        void Enter();
        void Update();
        void Exit();
    }

    public abstract class RoomStateBase : IRoomState {
        public PVPRoom room;
        public RoomStateBase(PVPRoom room) {
            this.room = room;
        }

        public abstract void Enter();

        public abstract void Exit();

        public abstract void Update();
    }

    public enum RoomStateEnum {
        None = 0,
        Confirm,    //确认
        Select,     //选择
        Load,       //加载
        Fight,      //战斗
        End,        //完成
    }
}
