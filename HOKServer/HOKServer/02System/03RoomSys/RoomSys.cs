/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 16:11
	功能: 房间系统 

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
    public class RoomSys : SystemRoot<RoomSys> {
        List<PVPRoom> pvpRoomLst = null;
        Dictionary<uint, PVPRoom> pvpRoomDic = null;
        public override void Init() {
            base.Init();
            pvpRoomLst = new List<PVPRoom>();
            pvpRoomDic = new Dictionary<uint, PVPRoom>();
            this.Log("RoomSys Init Done.");
        }

        public void AddPVPRoom(ServerSession[] sessionArr, PVPEnum pvp) {
            uint roomID = GetUniqueRoomID();
            PVPRoom room = new PVPRoom(roomID, pvp, sessionArr);
            pvpRoomLst.Add(room);
            pvpRoomDic.Add(roomID, room);
        }

        public override void Update() {
            base.Update();
        }

        public void SndConfirm(MsgPack pack) {
            SndConfirm req = pack.msg.sndConfirm;
            if(pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room)) {
                room.SndConfirm(pack.session);
            }
            else {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }

        public void SndSelect(MsgPack pack) {
            SndSelect req = pack.msg.sndSelect;
            if(pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room)) {
                room.SndSelect(pack.session, req.heroID);
            }
            else {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }

        public void SndLoadPrg(MsgPack pack) {
            SndLoadPrg req = pack.msg.sndLoadPrg;
            if(pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room)) {
                room.SndLoadPrg(pack.session, req.percent);
            }
            else {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }

        public void ReqBattleStart(MsgPack pack) {
            ReqBattleStart req = pack.msg.reqBattleStart;
            if(pvpRoomDic.TryGetValue(req.roomID, out PVPRoom room)) {
                room.ReqBattleStart(pack.session);
            }
            else {
                this.Warn("PVPRoom ID:" + req.roomID + " is destroyed.");
            }
        }

        uint roomID = 0;
        public uint GetUniqueRoomID() {
            roomID += 1;
            return roomID;
        }
    }
}
