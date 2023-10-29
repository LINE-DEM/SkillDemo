/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 子弹显示 

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class BulletView : ViewUnit {
    public override void Init(LogicUnit logicUnit) {
        base.Init(logicUnit);
    }

    public void DestroyBullet() {
        Destroy(gameObject, 0.1f);
    }
}
