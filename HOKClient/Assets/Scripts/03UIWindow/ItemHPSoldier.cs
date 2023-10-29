/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 小兵血条

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemHPSoldier : ItemHP {
    public Image iconState;

    public override void InitItem(MainLogicUnit unit, Transform root, int hp) {
        base.InitItem(unit, root, hp);

        SetActive(iconState, false);
        if(isFriend) {
            SetSprite(imgPrg, "ResImages/PlayWnd/selfteamhpfg");
        }
        else {
            SetSprite(imgPrg, "ResImages/PlayWnd/enemyteamhpfg");
        }
    }

    public override void SetStateInfo(StateEnum state, bool show) {
        base.SetStateInfo(state, show);

        if(!show) {
            SetActive(iconState, false);
        }
        else {
            //血条下方图标显示
            switch(state) {
                case StateEnum.Silenced:
                    SetSprite(iconState, "ResImages/PlayWnd/silenceIcon");
                    break;
                case StateEnum.Knockup:
                    SetSprite(iconState, "ResImages/PlayWnd/stunIcon");
                    break;
                case StateEnum.Stunned:
                    SetSprite(iconState, "ResImages/PlayWnd/stunIcon");
                    break;
                //TODO
                case StateEnum.Invincible:
                case StateEnum.Restricted:
                case StateEnum.None:
                default:
                    break;
            }

            SetActive(iconState);
            iconState.SetNativeSize();
        }
    }
}
