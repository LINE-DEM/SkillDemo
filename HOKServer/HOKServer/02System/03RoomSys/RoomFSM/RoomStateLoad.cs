/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/06 2:08
	功能: 战斗加载

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信服务号: qiqikertuts
           关注微信公众号: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace HOKServer {
    public class RoomStateLoad : RoomStateBase {
        private int[] percentArr;
        private bool[] loadArr;

        public RoomStateLoad(PVPRoom room) : base(room) {
        }

        public override void Enter() {
            int len = room.sessionArr.Length;
            percentArr = new int[len];
            loadArr = new bool[len];

            HOKMsg msg = new HOKMsg {
                cmd = CMD.NtfLoadRes,
                ntfLoadRes = new NtfLoadRes {
                    mapID = 101,//默认地图
                    heroList = new List<BattleHeroData>(),
                }
            };
            for(int i = 0; i < room.SelectArr.Length; i++) {
                SelectData sd = room.SelectArr[i];
                BattleHeroData hero = new BattleHeroData {
                    heroID = sd.selectID,
                    userName = GetUserName(i)
                };
                msg.ntfLoadRes.heroList.Add(hero);
            }

            for(int i = 0; i < len; i++) {
                msg.ntfLoadRes.posIndex = i;
                room.sessionArr[i].SendMsg(msg);
            }
        }

        public void UpdateLoadState(int posIndex, int percent) {
            percentArr[posIndex] = percent;
            HOKMsg msg = new HOKMsg {
                cmd = CMD.NtfLoadPrg,
                ntfLoadPrg = new NtfLoadPrg {
                    percentLst = new List<int>()
                }
            };
            for(int i = 0; i < percentArr.Length; i++) {
                msg.ntfLoadPrg.percentLst.Add(percentArr[i]);
            }

            room.BroadcastMsg(msg);
        }

        public void UpdateLoadDone(int posIndex) {
            loadArr[posIndex] = true;

            for(int i = 0; i < loadArr.Length; i++) {
                if(loadArr[i] == false) {
                    return;
                }
            }

            //全部加载完成
            HOKMsg msg = new HOKMsg {
                cmd = CMD.RspBattleStart
            };
            room.BroadcastMsg(msg);

            room.ChangeRoomState(RoomStateEnum.Fight);
            this.ColorLog(PEUtils.LogColor.Green, "RoomID:{0} 所有玩家加载完成，进入战斗。", room.roomID);
        }

        public override void Exit() {
            percentArr = null;
            loadArr = null;
        }

        public override void Update() {
        }

        string GetUserName(int posIndex) {
            UserData ud = CacheSvc.Instance.GetUserDataBySession(room.sessionArr[posIndex]);
            if(ud != null) {
                return ud.name;
            }
            return "";
        }
    }
}
