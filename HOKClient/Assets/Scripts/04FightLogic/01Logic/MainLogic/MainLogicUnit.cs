/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 主要逻辑单位（英雄，小兵，塔）

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;
using UnityEngine;

public enum UnitStateEnum {
    Alive,
    Dead
}

public enum UnitTypeEnum {
    Hero,
    Soldier,
    Tower,
}

public enum TeamEnum {
    None,
    Blue,
    Red,
    Neutal//中立，暂时用不上
}

public abstract partial class MainLogicUnit : LogicUnit {
    public LogicUnitData ud;
    public UnitStateEnum unitState;
    public UnitTypeEnum unitType;

    protected string pathPrefix = "";
    public MainViewUnit mainViewUnit;

    public MainLogicUnit(LogicUnitData ud) {
        this.ud = ud;
        unitName = ud.unitCfg.unitName;
    }


    public override void LogicInit() {
        //初始化属性
        InitProperties();
        //初始化技能
        InitSkill();
        //初始化移动
        InitMove();

        GameObject go = ResSvc.Instance.LoadPrefab(pathPrefix + "/" + ud.unitCfg.resName);
        mainViewUnit = go.GetComponent<MainViewUnit>();
        if(mainViewUnit == null) {
            this.Error("Get MainViewUnit Error:" + unitName);
        }
        mainViewUnit.Init(this);

        unitState = UnitStateEnum.Alive;
    }

    public override void LogicTick() {
        TickSkill();
        TickMove();
    }

    public override void LogicUnInit() {
        //UnInitSkill();
        UnInitMove();
    }

    public void InputKey(OpKey key) {
        switch(key.keyType) {
            case KeyType.Skill:
                InputSkillKey(key.skillKey);
                break;
            case KeyType.Move:
                PEInt x = PEInt.zero;
                x.ScaledValue = key.moveKey.x;
                PEInt z = PEInt.zero;
                z.ScaledValue = key.moveKey.z;
                InputMoveKey(new PEVector3(x, 0, z));
                break;
            case KeyType.None:
            default:
                this.Error("KEY is not exist");
                break;
        }
    }

    public void PlayAni(string aniName) {
        mainViewUnit.PlayAni(aniName);
    }

    public void PlayAudio(string audioName, bool loop = false, int delay = 0) {
        mainViewUnit.PlayAudio(audioName, loop, delay);
    }

    public virtual bool IsPlayerSelf() {
        return false;
    }
}
