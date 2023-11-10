/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/02 8:57
	功能: 客户端入口

    //=================*=================\\
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEUtils;
using System.Collections.Generic;
using JKFrame;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    public static GameRoot Instance;
    public Transform uiRoot;
    public TipsWnd tipsWnd;

    List<MonoTimer> tempTimerLst;
    List<MonoTimer> timerLst;

    void Start() {
        Instance = this;

        LogConfig cfg = new LogConfig {
            enableLog = true,
            logPrefix = "",
            enableTime = true,
            logSeparate = ">",
            enableThreadID = false,
            enableTrace = true,
            enableSave = true,
            enableCover = true,
            saveName = "HOKClientPELog.txt",
            loggerType = LoggerType.Unity
        };
        PELog.InitSettings(cfg);
        PELog.ColorLog(LogColor.Green, "InitLogger.");
        ///随机种子（TODO:服务器统一下发种子数据）
        RandomUtils.InitRandom(666);
        DontDestroyOnLoad(this);
        InitRoot();
        PELog.Log("Init Root.");
        Init();
        PELog.Log("Init Done.");
    }

    // Update is called once per frame
    void Update() {
        //View Timer Tick
        if(tempTimerLst.Count > 0) {
            timerLst.AddRange(tempTimerLst);
            tempTimerLst.Clear();
        }

        for(int i = timerLst.Count - 1; i >= 0; --i) {
            MonoTimer timer = timerLst[i];
            if(timer.IsActive) {
                timer.TickTimer(Time.deltaTime * 1000);
            }
            else {
                timerLst.RemoveAt(i);
            }
        }
    }

    void InitRoot() {
        for(int i = 0; i < uiRoot.childCount; i++) {
            Transform trans = uiRoot.GetChild(i);
            trans.gameObject.SetActive(false);
        }
        tipsWnd.SetWndState();
    }

    private NetSvc netSvc;
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    void Init() {
        //计时器
        timerLst = new List<MonoTimer>();
        tempTimerLst = new List<MonoTimer>();

        netSvc = GetComponent<NetSvc>();
        netSvc.InitSvc();
        resSvc = GetComponent<ResSvc>();
        resSvc.InitSvc();
        audioSvc = GetComponent<AudioSvc>();
        audioSvc.InitSvc();

        GMSystem gm = GetComponent<GMSystem>();
        gm.InitSys();


        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();
        LobbySys lobby = GetComponent<LobbySys>();
        lobby.InitSys();
        BattleSys battle = GetComponent<BattleSys>();
        battle.InitSys();
        PoolManager pool = GetComponent<PoolManager>();
        pool.Init();
        //login
        PELog.Log("EnterLogin.");
        login.EnterLogin();
    }

    public void ShowTips(string tips) {
        tipsWnd.AddTips(tips);
    }

    public void AddMonoTimer(MonoTimer timer) {
        tempTimerLst.Add(timer);
    }


    #region
    UserData userData;
    public UserData UserData {
        set { userData = value; }
        get { return userData; }
    }
    private uint roomID;
    public uint RoomID {
        set { roomID = value; }
        get { return roomID; }
    }
    private int mapID;
    public int MapID {
        set { mapID = value; }
        get { return mapID; }
    }
    private List<BattleHeroData> heroLst;
    public List<BattleHeroData> HeroLst {
        set { heroLst = value; }
        get { return heroLst; }
    }

    private int selfIndex;
    public int SelfIndex {
        set { selfIndex = value; }
        get { return selfIndex; }
    }
    #endregion
}
