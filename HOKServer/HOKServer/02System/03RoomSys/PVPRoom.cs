/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/26 17:24
	功能: 对战房间

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PENet;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer {
    public class PVPRoom {
        public uint roomID;
        public PVPEnum pvpEnum = PVPEnum.None;
        public ServerSession[] sessionArr;
        private SelectData[] selectArr = null;
        public SelectData[] SelectArr {
            set {
                selectArr = value;
            }
            get {
                return selectArr;
            }
        }

        private Dictionary<RoomStateEnum, RoomStateBase> fsm = new Dictionary<RoomStateEnum, RoomStateBase>();
        private RoomStateEnum currentRoomStateEnum = RoomStateEnum.None;

        public PVPRoom(uint roomID, PVPEnum pvpEnum, ServerSession[] sessionArr) {
            this.roomID = roomID;
            this.pvpEnum = pvpEnum;
            this.sessionArr = sessionArr;

            fsm.Add(RoomStateEnum.Confirm, new RoomStateConfirm(this));
            fsm.Add(RoomStateEnum.Select, new RoomStateSelect(this));
            fsm.Add(RoomStateEnum.Load, new RoomStateLoad(this));
            fsm.Add(RoomStateEnum.Fight, new RoomStateFight(this));
            fsm.Add(RoomStateEnum.End, new RoomStateEnd(this));

            ChangeRoomState(RoomStateEnum.Confirm);
        }

        public void ChangeRoomState(RoomStateEnum targetState) {
            if(currentRoomStateEnum == targetState) {
                return;
            }

            if(fsm.ContainsKey(targetState)) {
                if(currentRoomStateEnum != RoomStateEnum.None) {
                    fsm[currentRoomStateEnum].Exit();
                }
                fsm[targetState].Enter();
                currentRoomStateEnum = targetState;
            }
        }

        public void BroadcastMsg(HOKMsg msg) {
            byte[] bytes = KCPTool.Serialize(msg);
            if(bytes != null) {
                for(int i = 0; i < sessionArr.Length; i++) {
                    sessionArr[i].SendMsg(bytes);
                }
            }
        }

        int GetPosIndex(ServerSession session) {
            int posIndex = 0;
            for(int i = 0; i < sessionArr.Length; i++) {
                if(sessionArr[i].Equals(session)) {
                    posIndex = i;
                }
            }
            return posIndex;
        }

        public void SndConfirm(ServerSession session) {
            if(currentRoomStateEnum == RoomStateEnum.Confirm) {
                if(fsm[currentRoomStateEnum] is RoomStateConfirm state) {
                    state.UpdateConfirmState(GetPosIndex(session));
                }
            }
        }

        public void SndSelect(ServerSession session, int heroID) {
            if(currentRoomStateEnum == RoomStateEnum.Select) {
                if(fsm[currentRoomStateEnum] is RoomStateSelect state) {
                    state.UpdateHeroSelect(GetPosIndex(session), heroID);
                }
            }
        }
        public void SndLoadPrg(ServerSession session, int percent) {
            if(currentRoomStateEnum == RoomStateEnum.Load) {
                if(fsm[currentRoomStateEnum] is RoomStateLoad state) {
                    state.UpdateLoadState(GetPosIndex(session), percent);
                }
            }
        }

        public void ReqBattleStart(ServerSession session) {
            if(currentRoomStateEnum == RoomStateEnum.Load) {
                if(fsm[currentRoomStateEnum] is RoomStateLoad state) {
                    state.UpdateLoadDone(GetPosIndex(session));
                }
            }
        }

    }
}
