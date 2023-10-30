/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: GM战斗模拟

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSystem : SysRoot {
    public static GMSystem Instance;
    public bool isActive = false;

    private uint frameID = 0;
    private List<OpKey> opkeyLst = new List<OpKey>();

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        this.Log("Init GMSystem Done.");
    }

    public void StartSimulate() {
        isActive = true;
        StartCoroutine(BattleSimulate());
    }

    public IEnumerator BattleSimulate() {
        SimulateLoadRes();
        yield return new WaitForSeconds(0.5f);
        SimulateBattleStart();
    }

    void SimulateLoadRes() {
        HOKMsg msg = new HOKMsg {
            cmd = CMD.NtfLoadRes,
            ntfLoadRes = new NtfLoadRes {
                mapID = 102,
                heroList = new List<BattleHeroData> {
                    new BattleHeroData{ heroID = 102,userName = "F1"},
                    new BattleHeroData{ heroID = 101,userName = "F2"},
                    new BattleHeroData{ heroID = 101,userName = "F3"},
                    new BattleHeroData{ heroID = 102,userName = "F4"},
                    new BattleHeroData{ heroID = 102,userName = "F5"},
                    new BattleHeroData{ heroID = 102,userName = "F6"},
                },
                posIndex = 0
            }
        };
        LobbySys.Instance.NtfLoadRes(msg);
    }

    void SimulateBattleStart() {
        HOKMsg msg = new HOKMsg {
            cmd = CMD.RspBattleStart
        };
        BattleSys.Instance.RspBattleStart(msg);
    }

    public void SimulateServerRcvMsg(HOKMsg msg) {
        switch(msg.cmd) {
            case CMD.SndOpKey:
                UpdateOpeKey(msg.sndOpKey.opKey);
                break;
            default:
                break;
        }
    }

    void FixedUpdate() {
        ++frameID;
        HOKMsg msg = new HOKMsg {
            cmd = CMD.NtfOpKey,
            ntfOpKey = new NtfOpKey {
                frameID = frameID,
                keyList = new List<OpKey>()
            }
        };

        int count = opkeyLst.Count;
        if(count > 0) {
            for(int i = 0; i < opkeyLst.Count; i++) {
                OpKey key = opkeyLst[i];
                msg.ntfOpKey.keyList.Add(key);
            }
        }
        opkeyLst.Clear();
        netSvc.AddMsgQue(msg);
    }


    void UpdateOpeKey(OpKey key) {
        opkeyLst.Add(key);
    }
}
