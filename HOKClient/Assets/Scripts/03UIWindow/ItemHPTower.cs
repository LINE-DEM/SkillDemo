/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 塔血条显示 

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using UnityEngine;

public class ItemHPTower : ItemHP {
    public override void InitItem(MainLogicUnit unit, Transform root, int hp) {
        base.InitItem(unit, root, hp);
        if(isFriend) {
            SetSprite(imgPrg, "ResImages/PlayWnd/selftowerhpfg");
        }
        else {
            SetSprite(imgPrg, "ResImages/PlayWnd/enemytowerhpfg");
        }
    }
}
