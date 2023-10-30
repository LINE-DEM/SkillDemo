/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/06 2:06
	功能: 等待确认

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
    public class RoomStateConfirm : RoomStateBase {
        private ConfirmData[] confirmArr = null;
        private int checkTaskID = -1;
        private bool isAllConfirmed = false;

        public RoomStateConfirm(PVPRoom room) : base(room) {
        }

        public override void Enter() {
            int len = room.sessionArr.Length;
            confirmArr = new ConfirmData[len];
            for(int i = 0; i < len; i++) {
                confirmArr[i] = new ConfirmData {
                    iconIndex = i,
                    confirmDone = false
                };
            }

            HOKMsg msg = new HOKMsg {
                cmd = CMD.NtfConfirm,
                ntfConfirm = new NtfConfirm {
                    roomID = room.roomID,
                    dissmiss = false,
                    confirmArr = confirmArr
                }
            };

            room.BroadcastMsg(msg);

            checkTaskID = TimerSvc.Instance.AddTask(ServerConfig.ConfirmCountDown * 1000, ReachTimeLimit);
        }

        void ReachTimeLimit(int tid) {
            if(isAllConfirmed) {
                return;
            }
            else {
                this.ColorLog(PEUtils.LogColor.Yellow, "RoomID:{0} 确认超时，解散房间，重新匹配。", room.roomID);
                HOKMsg msg = new HOKMsg {
                    cmd = CMD.NtfConfirm,
                    ntfConfirm = new NtfConfirm {
                        dissmiss = true
                    }
                };

                room.BroadcastMsg(msg);
                room.ChangeRoomState(RoomStateEnum.End);
            }
        }

        void CheckConfirmState() {
            for(int i = 0; i < confirmArr.Length; i++) {
                if(confirmArr[i].confirmDone == false) {
                    return;
                }
            }
            isAllConfirmed = true;
        }

        public void UpdateConfirmState(int posIndex) {
            confirmArr[posIndex].confirmDone = true;
            CheckConfirmState();

            if(isAllConfirmed) {
                if(TimerSvc.Instance.DeleteTask(checkTaskID)) {
                    this.ColorLog(PEUtils.LogColor.Green, "RoomID:{0} 所有玩家确认完成，进入英雄选择。", room.roomID);
                }
                else {
                    this.Warn("Remove CheckTaskID Failed.");
                }
                room.ChangeRoomState(RoomStateEnum.Select);
            }
            else {
                HOKMsg msg = new HOKMsg {
                    cmd = CMD.NtfConfirm,
                    ntfConfirm = new NtfConfirm {
                        roomID = room.roomID,
                        dissmiss = false,
                        confirmArr = confirmArr
                    }
                };

                room.BroadcastMsg(msg);
            }
        }

        public override void Exit() {
            confirmArr = null;
            checkTaskID = -1;
            isAllConfirmed = false;
        }

        public override void Update() {
        }
    }
}
