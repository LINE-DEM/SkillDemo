/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/09 21:29
	功能: 战斗系统

    //=================*=================\\
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;
using PEPhysx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSys : SysRoot {
    public static BattleSys Instance;
    public LoadWnd loadWnd;
    public PlayWnd playWnd;
    public HPWnd hpWnd;

    public float SkillDisMultipler;

    public bool isTickFight;
    private int mapID;

    private List<BattleHeroData> heroLst = null;
    private GameObject fightGO;
    private FightMgr fightMgr;
    private AudioSource battleAudio;
    uint keyID = 0;
    public uint KeyID {
        get {
            return ++keyID;
        }
    }

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        this.Log("Init BattleSys Done.");
    }

    public void EnterBattle() {
        audioSvc.StopBGMusic();
        loadWnd.SetWndState();

        mapID = root.MapID;
        heroLst = root.HeroLst;
        resSvc.AsyncLoadScene("map_" + mapID, SceneLoadProgress, SceneLoadDone);
    }

    int lastPercent = 0;
    void SceneLoadProgress(float val) {
        int percent = (int)(val * 100);
        if(lastPercent != percent) {
            HOKMsg msg = new HOKMsg {
                cmd = CMD.SndLoadPrg,
                sndLoadPrg = new SndLoadPrg {
                    roomID = root.RoomID,
                    percent = percent
                }
            };
            netSvc.SendMsg(msg);
            lastPercent = percent;
        }
    }

    void SceneLoadDone() {
        //初始化UI
        playWnd.SetWndState();
        hpWnd.SetWndState();
        //加载角色及资源
        //初始化战斗
        fightGO = new GameObject {
            name = "fight"
        };
        fightMgr = fightGO.AddComponent<FightMgr>();
        battleAudio = fightGO.AddComponent<AudioSource>();
        MapCfg mapCfg = resSvc.GetMapConfigByID(mapID);
        fightMgr.Init(heroLst, mapCfg);

        HOKMsg msg = new HOKMsg {
            cmd = CMD.ReqBattleStart,
            reqBattleStart = new ReqBattleStart {
                roomID = root.RoomID
            }
        };
        netSvc.SendMsg(msg);
    }

    public void NtfLoadPrg(HOKMsg msg) {
        loadWnd.RefreshPrgData(msg.ntfLoadPrg.percentLst);
    }

    public void RspBattleStart(HOKMsg msg) {
        fightMgr.InitCamFollowTrans(root.SelfIndex);
        playWnd.InitSkillInfo();
        loadWnd.SetWndState(false);
        audioSvc.PlayBGMusic(NameDefine.BattleBGMusic);
        isTickFight = true;
    }

    public void NtfOpKey(HOKMsg msg) {
        if(isTickFight) {
            fightMgr.InputKey(msg.ntfOpKey.keyList);
            fightMgr.Tick();
        }
    }

    public bool CheckUIInput() {
        return playWnd.IsUIInput();
    }

    public void EnterCDState(int skilldID, int cdTime) {
        playWnd.EnterCDState(skilldID, cdTime);
    }

    public MainLogicUnit GetSelfHero() {
        return fightMgr.GetSelfHero(root.SelfIndex);
    }

    public TeamEnum GetCurrentUserTeam() {
        int sep = heroLst.Count / 2;
        if(root.SelfIndex < sep) {
            return TeamEnum.Blue;
        }
        else {
            return TeamEnum.Red;
        }
    }

    public List<PEColliderBase> GetEnvColliders() {
        return fightMgr.GetEnvColliders();
    }

    public void AddBullet(Bullet bullet) {
        fightMgr.AddBullet(bullet);
    }

    #region API Func
    /// <summary>
    /// 发送移动帧操作到服务器
    /// </summary>
    /// <param name="logicDir"></param>
    /// <returns></returns>
    public bool SendMoveKey(PEVector3 logicDir) {
        if(CanMove()) {
            HOKMsg msg = new HOKMsg {
                cmd = CMD.SndOpKey,
                sndOpKey = new SndOpKey {
                    roomID = root.RoomID,
                    opKey = new OpKey {
                        opIndex = root.SelfIndex,
                        keyType = KeyType.Move,
                        moveKey = new MoveKey()
                    }
                }
            };
            msg.sndOpKey.opKey.moveKey.x = logicDir.x.ScaledValue;
            msg.sndOpKey.opKey.moveKey.z = logicDir.z.ScaledValue;
            msg.sndOpKey.opKey.moveKey.keyID = KeyID;
            NetSvc.Instance.SendMsg(msg);
            return true;
        }
        else {
            return false;
        }
    }
    public void SendSkillKey(int skillID) {
        SendSkillKey(skillID, Vector3.zero);
    }
    //TODO 发送技能施放指令
    public void SendSkillKey(int skillID, Vector3 vec) {
        //this.Log($"Rls Skill:{skillID} with Data:{vec}");
        if(CanReleaseSkill(skillID)) {
            HOKMsg netSkillMsg = new HOKMsg {
                cmd = CMD.SndOpKey,
                sndOpKey = new SndOpKey {
                    roomID = root.RoomID,
                    opKey = new OpKey {
                        opIndex = root.SelfIndex,
                        keyType = KeyType.Skill,
                        skillKey = new SkillKey {
                            skillID = (uint)skillID,
                            x_value = ((PEInt)vec.x).ScaledValue,
                            z_value = ((PEInt)vec.z).ScaledValue,
                        }
                    }
                }
            };
            netSvc.SendMsg(netSkillMsg);
        }
        else {
            this.Log("skill can not release.");
        }
    }
    bool CanReleaseSkill(int skillID) {
        return fightMgr.CanReleaseSkill(root.SelfIndex, skillID);
    }
    public bool IsForbidSelfPlayerReleaseSkill() {
        return fightMgr.IsForbidReleaseSkill(root.SelfIndex);
    }

    bool CanMove() {
        return fightMgr.CanMove(root.SelfIndex);
    }
    #endregion
}
