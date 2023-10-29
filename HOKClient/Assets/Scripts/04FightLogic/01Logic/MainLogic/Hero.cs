/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 英雄单位

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class Hero : MainLogicUnit {
    public int heroID;
    public int posIndex;
    public string userName;//玩家名字

    public Hero(HeroData hd) : base(hd) {
        heroID = hd.heroID;
        posIndex = hd.posIndex;
        userName = hd.userName;

        unitType = UnitTypeEnum.Hero;
        unitName = ud.unitCfg.unitName + "_" + userName;

        pathPrefix = "ResChars";
    }

    #region API Functions
    public override bool IsPlayerSelf() {
        return posIndex == GameRoot.Instance.SelfIndex;
    }
    #endregion
}
