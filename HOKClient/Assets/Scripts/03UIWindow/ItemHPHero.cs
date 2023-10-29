/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 英雄血条显示

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemHPHero : ItemHPSoldier {
    public Image imgState;
    public Text txtName;
    public Transform hpMarkRoot;
    public int markCount;

    public override void InitItem(MainLogicUnit unit, Transform root, int hp) {
        base.InitItem(unit, root, hp);

        SetActive(imgState, false);
        txtName.text = unit.unitName;
        SetActive(txtName);

        SetHPMark(hp);
    }

    void SetHPMark(int hp) {
        int count = hp / markCount;
        int childCount = hpMarkRoot.childCount;
        for(int i = 0; i < childCount; i++) {
            if(i < count) {
                SetActive(hpMarkRoot.GetChild(i));
            }
            else {
                SetActive(hpMarkRoot.GetChild(i), false);
            }
        }
    }

    public override void SetStateInfo(StateEnum state, bool show) {
        base.SetStateInfo(state, show);

        if(!show) {
            SetActive(txtName);
            SetActive(imgState, false);
        }
        else {
            switch(state) {
                case StateEnum.Silenced:
                    SetSprite(imgState, "ResImages/PlayWnd/silenceState");
                    break;
                case StateEnum.Knockup:
                    SetSprite(imgState, "ResImages/PlayWnd/knockState");
                    break;
                case StateEnum.Stunned:
                    SetSprite(imgState, "ResImages/PlayWnd/stunState");
                    break;
                //TODO
                case StateEnum.Invincible:
                case StateEnum.Restricted:
                case StateEnum.None:
                default:
                    break;
            }

            SetActive(txtName, false);
            SetActive(imgState);
            imgState.SetNativeSize();
        }
    }

}
