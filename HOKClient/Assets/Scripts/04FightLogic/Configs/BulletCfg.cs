/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 子弹配置

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class BulletCfg {
    public BulletTypeEnum bulletType;
    public string bulletName;
    public string resPath;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletHeight;
    public float bulletOffset;
    public int bulletDelay;//ms
    public bool canBlock;

    public TargetCfg impacter;
    public int bulletDuration;
}

public enum BulletTypeEnum {
    UIDirection,//ui指定方向
    UIPosition,//ui指定位置
    SkillTarget,//当前技能目标
    BuffSearch,
    //TODO
}