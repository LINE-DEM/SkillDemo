/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 血条显示Item

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using UnityEngine;
using UnityEngine.UI;

public abstract class ItemHP : WindowRoot {
    public RectTransform rect;
    public Image imgPrg;

    protected bool isFriend;
    Transform rootTrans;
    int hpVal;

    public virtual void InitItem(MainLogicUnit unit, Transform root, int hp) {
        TeamEnum selfTeam;
        if(BattleSys.Instance.GetCurrentUserTeam() == TeamEnum.Blue) {
            selfTeam = TeamEnum.Blue;
        }
        else {
            selfTeam = TeamEnum.Red;
        }
        isFriend = unit.IsTeam(selfTeam);

        imgPrg.fillAmount = 1;
        rootTrans = root;
        hpVal = hp;
    }

    public void UpdateHPPrg(int newVal) {
        if(newVal == 0) {
            SetActive(gameObject, false);
        }
        else {
            SetActive(gameObject);
        }
        imgPrg.fillAmount = newVal * 1.0f / hpVal;
    }

    public virtual void SetStateInfo(StateEnum state, bool show) { }

    private void Update() {
        if(rootTrans) {
            float scaleRate = 1.0F * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
            rect.anchoredPosition = screenPos * scaleRate;
        }
    }
}


public enum StateEnum {
    None,
    Silenced,
    Knockup,
    Stunned,

    Invincible,
    Restricted,
}
