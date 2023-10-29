/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/26 14:04
	功能: 开始界面

    //=================*=================\\
           教学官网：www.qiqiker.com
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWnd : WindowRoot {
    public Text txtName;

    private UserData ud = null;
    protected override void InitWnd() {
        base.InitWnd();
        ud = root.UserData;
        txtName.text = ud.name;
    }

    public void ClickStartBtn() {
        audioSvc.PlayUIAudio("com_click1");
        LoginSys.Instance.EnterLobby();
    }

    public void ClickAreaBtn() {
        root.ShowTips("正在开发中...");
    }

    public void ClickExitBtn() {
        root.ShowTips("正在开发中...");
    }
}
