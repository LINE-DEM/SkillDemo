/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/06 3:16
	功能: 匹配确认界面

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchWnd : WindowRoot {
    public Text txtTime;
    public Text txtConfirm;
    public Transform leftPlayerRoot;
    public Transform rightPlayerRoot;
    public Button btnCofirm;

    private int timeCount;
    protected override void InitWnd() {
        base.InitWnd();

        timeCount = ServerConfig.ConfirmCountDown;
        btnCofirm.interactable = true;
        audioSvc.PlayUIAudio("matchReminder");
    }

    public void RefreshUI(ConfirmData[] confirmArr) {
        int count = confirmArr.Length / 2;
        for(int i = 0; i < 5; i++) {
            Transform player = leftPlayerRoot.GetChild(i);
            if(i < count) {
                SetActive(player);
                string iconPath = "ResImages/MatchWnd/icon_" + confirmArr[i].iconIndex;
                string framePath = "ResImages/MatchWnd/frame_" + (confirmArr[i].confirmDone ? "sure" : "normal");
                Image imgIcon = GetImage(player);
                SetSprite(imgIcon, iconPath);
                Image imgFrame = GetImage(player, "img_state");
                SetSprite(imgFrame, framePath);
                imgFrame.SetNativeSize();
            }
            else {
                SetActive(player, false);
            }
        }

        for(int i = 0; i < 5; i++) {
            Transform player = rightPlayerRoot.GetChild(i);
            if(i < count) {
                SetActive(player);
                string iconPath = "ResImages/MatchWnd/icon_" + confirmArr[i + count].iconIndex;
                string framePath = "ResImages/MatchWnd/frame_" + (confirmArr[i + count].confirmDone ? "sure" : "normal");
                Image imgIcon = GetImage(player);
                SetSprite(imgIcon, iconPath);
                Image imgFrame = GetImage(player, "img_state");
                SetSprite(imgFrame, framePath);
                imgFrame.SetNativeSize();
            }
            else {
                SetActive(player, false);
            }
        }

        int cofirmCount = 0;
        for(int i = 0; i < confirmArr.Length; i++) {
            if(confirmArr[i].confirmDone) {
                ++cofirmCount;
            }
        }

        txtConfirm.text = cofirmCount + "/" + confirmArr.Length + "就绪";
    }

    public void ClickConfirmBtn() {
        audioSvc.PlayUIAudio("matchSureClick");

        HOKMsg msg = new HOKMsg {
            cmd = CMD.SndConfirm,
            sndConfirm = new SndConfirm {
                roomID = root.RoomID
            }
        };

        netSvc.SendMsg(msg);
        btnCofirm.interactable = false;
    }

    private float deltaCount;
    private void Update() {
        float delta = Time.deltaTime;
        deltaCount += delta;
        if(deltaCount >= 1) {
            deltaCount -= 1;
            timeCount -= 1;
            if(timeCount < 0) {
                timeCount = 0;
            }
            txtTime.text = timeCount.ToString();
        }
    }
}
