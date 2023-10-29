/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/09 21:29
	功能: 大厅系统 

    //=================*=================\\
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;

public class LobbySys : SysRoot {
    public static LobbySys Instance;
    public LobbyWnd lobbyWnd;
    public MatchWnd matchWnd;
    public SelectWnd selectWnd;

    public override void InitSys() {
        base.InitSys();

        Instance = this;
        this.Log("Init LobbySys Done.");
    }

    public void EnterLobby() {
        lobbyWnd.SetWndState();
    }

    public void RspMatch(HOKMsg msg) {
        int predictTime = msg.rspMatch.predictTime;
        lobbyWnd.ShowMatchInfo(true, predictTime);
    }

    public void NtfConfirm(HOKMsg msg) {
        NtfConfirm ntf = msg.ntfConfirm;

        if(ntf.dissmiss) {
            matchWnd.SetWndState(false);
            lobbyWnd.SetWndState();
        }
        else {
            root.RoomID = ntf.roomID;
            lobbyWnd.SetWndState(false);
            if(matchWnd.gameObject.activeSelf == false) {
                matchWnd.SetWndState();
            }
            matchWnd.RefreshUI(ntf.confirmArr);
        }
    }

    public void NtfSelect(HOKMsg msg) {
        matchWnd.SetWndState(false);
        selectWnd.SetWndState();
    }

    public void NtfLoadRes(HOKMsg msg) {
        root.MapID = msg.ntfLoadRes.mapID;
        root.HeroLst = msg.ntfLoadRes.heroList;
        root.SelfIndex = msg.ntfLoadRes.posIndex;
        selectWnd.SetWndState(false);
        BattleSys.Instance.EnterBattle();
    }
}
