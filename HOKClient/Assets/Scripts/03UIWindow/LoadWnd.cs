/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/08 2:46
	功能: 加载窗口

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

public class LoadWnd : WindowRoot {
    public Transform blueTeamRoot;
    public Transform redTeamRoot;

    private List<BattleHeroData> heroLst;
    private List<Text> txtPercentLst;

    protected override void InitWnd() {
        base.InitWnd();

        txtPercentLst = new List<Text>();
        audioSvc.PlayUIAudio("load");

        heroLst = root.HeroLst;
        int count = heroLst.Count / 2;
        //blueteam
        for(int i = 0; i < 5; i++) {
            Transform player = blueTeamRoot.GetChild(i);
            if(i < count) {
                SetActive(player);
                UnitCfg cfg = resSvc.GetUnitConfigByID(heroLst[i].heroID);
                SetSprite(GetImage(player, "imgHero"), "ResImages/LoadWnd/" + cfg.resName + "_load");
                SetText(GetText(player, "txtHeroName"), cfg.unitName);
                SetText(GetText(player, "bgName/txtPlayerName"), heroLst[i].userName);
                Text txtPrg = GetText(player, "txtProgress");
                txtPercentLst.Add(txtPrg);
                SetText(txtPrg, "0%");
            }
            else {
                SetActive(player, false);
            }
        }

        //redteam
        for(int i = 0; i < 5; i++) {
            Transform player = redTeamRoot.GetChild(i);
            if(i < count) {
                SetActive(player);
                UnitCfg cfg = resSvc.GetUnitConfigByID(heroLst[i + count].heroID);
                SetSprite(GetImage(player, "imgHero"), "ResImages/LoadWnd/" + cfg.resName + "_load");
                SetText(GetText(player, "txtHeroName"), cfg.unitName);
                SetText(GetText(player, "bgName/txtPlayerName"), heroLst[i + count].userName);
                Text txtPrg = GetText(player, "txtProgress");
                txtPercentLst.Add(txtPrg);
                SetText(txtPrg, "0%");
            }
            else {
                SetActive(player, false);
            }
        }

    }

    public void RefreshPrgData(List<int> percentLst) {
        for(int i = 0; i < percentLst.Count; i++) {
            txtPercentLst[i].text = percentLst[i] + "%";
        }
    }
}
