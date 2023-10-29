/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: Buff效果表现

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class BuffView : ViewUnit {
    Buff buff;
    public override void Init(LogicUnit buff) {
        base.Init(buff);
        this.buff = buff as Buff;

        if(this.buff.cfg.staticPosType != StaticPosTypeEnum.None) {
            //固定位置buff
            transform.position = buff.LogicPos.ConvertViewVector3();
            transform.rotation = CalcRotation(buff.LogicDir.ConvertViewVector3());
        }
    }

    //用一个空函数覆盖位置与方向的更新
    protected override void Update() { }

    public void DestroyBuff() {
        Destroy(gameObject, 0.1f);
    }
}
