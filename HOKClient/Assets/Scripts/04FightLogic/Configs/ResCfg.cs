/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/03/08 0:31
	功能: 配置

    //=================*=================\\
           教学官网：www.qiqiker.com
           关注微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using PEPhysx;

public class HeroData : LogicUnitData {
    public int heroID;
    public int posIndex;
    public string userName;//玩家名字
}

public class LogicUnitData {
    public TeamEnum teamEnum;
    public PEVector3 bornPos;
    public UnitCfg unitCfg;
}

public class UnitCfg {
    public int unitID;
    public string unitName;//单位角色名字
    public string resName;//资源
    public PEInt hitHeight;
    //核心属性
    public int hp;
    public int def;
    public int moveSpeed;

    //碰撞体
    public ColliderConfig colliCfg;

    //技能ID数组
    public int[] pasvBuff;
    public int[] skillArr;
}

public class MapCfg {
    public int mapID;
    public PEVector3 blueBorn;
    public PEVector3 redBorn;

    //小兵出生间隔
    public int bornDelay;
    public int bornInterval;
    public int waveInterval;
}
