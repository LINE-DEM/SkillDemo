/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 18:03
	功能: 网络通信数据协议

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信公众号: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PENet;
using System;
using System.Collections.Generic;

namespace HOKProtocol {
    [Serializable]
    public class HOKMsg : KCPMsg {
        public CMD cmd;
        public ErrorCode error;
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqMatch reqMatch;
        public RspMatch rspMatch;
        public NtfConfirm ntfConfirm;
        public SndConfirm sndConfirm;
        //public NtfSelect ntfSelect;
        public SndSelect sndSelect;
        public NtfLoadRes ntfLoadRes;
        public SndLoadPrg sndLoadPrg;
        public NtfLoadPrg ntfLoadPrg;

        public ReqBattleStart reqBattleStart;
        public RspBatlleStart rspBatlleStart;

        public SndOpKey sndOpKey;
        public NtfOpKey ntfOpKey;
    }

    #region 登录相关
    [Serializable]
    public class ReqLogin {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin {
        public UserData userData;
    }

    [Serializable]
    public class UserData {
        public uint id;
        public string name;
        public int lv;
        public int exp;
        public int coin;
        public int diamond;
        public int ticket;
        public List<HeroSelectData> heroSelectData;
    }

    [Serializable]
    public class HeroSelectData {
        public int heroID;
        //已拥有
        //本周限名
        //体验卡
    }
    #endregion

    #region 匹配确认
    [Serializable]
    public enum PVPEnum {
        None = 0,
        _1V1 = 1,
        _2V2 = 2,
        _5V5 = 3
    }
    [Serializable]
    public class ReqMatch {
        public PVPEnum pvpEnum;
    }

    [Serializable]
    public class RspMatch {
        public int predictTime;
    }

    [Serializable]
    public class NtfConfirm {
        public uint roomID;
        public bool dissmiss;//解散
        public ConfirmData[] confirmArr;
    }
    [Serializable]
    public class ConfirmData {
        public int iconIndex;
        public bool confirmDone;
    }
    [Serializable]
    public class SndConfirm {
        public uint roomID;
    }
    #endregion

    #region 选择加载
    //[Serializable]
    //public class NtfSelect {

    //}
    [Serializable]
    public class SelectData {
        public int selectID;
        public bool selectDone;
    }
    [Serializable]
    public class SndSelect {
        public uint roomID;
        public int heroID;
    }
    [Serializable]
    public class NtfLoadRes {
        public int mapID;
        public List<BattleHeroData> heroList;
        public int posIndex;
    }
    [Serializable]
    public class BattleHeroData {
        public string userName;//玩家名字
        public int heroID;
        //级别，皮肤ID,边框，称号TODO
    }
    [Serializable]
    public class SndLoadPrg {
        public uint roomID;
        public int percent;
    }
    [Serializable]
    public class NtfLoadPrg {
        public List<int> percentLst;
    }
    #endregion

    #region 核心战斗
    [Serializable]
    public class ReqBattleStart {
        public uint roomID;
    }
    [Serializable]
    public class RspBatlleStart {

    }

    [Serializable]
    public class SndOpKey {
        public uint roomID;
        public OpKey opKey;
    }
    [Serializable]
    public class NtfOpKey {
        public uint frameID;
        public List<OpKey> keyList;
    }


    #endregion

    //错误码
    public enum ErrorCode {
        None,
        AcctIsOnline,
    }

    //通信协议命令号
    public enum CMD {
        None = 0,
        //登录
        ReqLogin = 1,
        RspLogin = 2,

        //匹配
        ReqMatch = 3,
        RspMatch = 4,

        //确认
        NtfConfirm = 5,
        SndConfirm = 6,

        //选择
        NtfSelect = 7,
        SndSelect = 8,

        //加载
        NtfLoadRes = 9,
        SndLoadPrg = 10,
        NtfLoadPrg = 11,

        //战斗
        ReqBattleStart = 12,
        RspBattleStart = 13,

        //操作码
        SndOpKey = 100,
        NtfOpKey = 101
    }

}
