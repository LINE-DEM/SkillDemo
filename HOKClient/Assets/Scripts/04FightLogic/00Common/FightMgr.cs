/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: PVP战斗管理器

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using PEPhysx;
using HOKProtocol;
using UnityEngine;
using System.Collections.Generic;

public class FightMgr : MonoBehaviour {
    MapRoot mapRoot;
    EnvColliders logicEnv;

    Transform transFollow;

    //英雄角色集合
    List<Hero> heroLst;
    List<Bullet> bulletLst;
    public Dictionary<int,Bullet> bulletDic;
    private int resID = 0;
    
    public void Init(List<BattleHeroData> battleHeroLst, MapCfg mapCfg) {
        heroLst = new List<Hero>();
        bulletLst = new List<Bullet>();
        //初始化碰撞环境
        InitEnv();
        //防御塔
        //英雄
        InitHero(battleHeroLst, mapCfg);
        //小兵

        //delay 以后 出生小兵。。。
    }

    public void UnInit() {
        heroLst.Clear();
        bulletLst.Clear();
    }

    void InitHero(List<BattleHeroData> battleHeroLst, MapCfg mapCfg) {
        int sep = battleHeroLst.Count / 2;
        Hero[] blueTeamHero = new Hero[sep];
        Hero[] redTeamHero = new Hero[sep];
        for(int i = 0; i < battleHeroLst.Count; i++) {
            HeroData hd = new HeroData {
                heroID = battleHeroLst[i].heroID,
                posIndex = i,
                userName = battleHeroLst[i].userName,
                unitCfg = ResSvc.Instance.GetUnitConfigByID(battleHeroLst[i].heroID)
            };

            Hero hero;
            if(i < sep) {
                hd.teamEnum = TeamEnum.Blue;
                hd.bornPos = mapCfg.blueBorn + new PEVector3(0, 0, i * (PEInt)1.5f);
                hero = new Hero(hd);
                blueTeamHero[i] = hero;
            }
            else {
                hd.teamEnum = TeamEnum.Red;
                hd.bornPos = mapCfg.redBorn + new PEVector3(0, 0, (i - sep) * (PEInt)1.5f);
                hero = new Hero(hd);
                redTeamHero[i - sep] = hero;
            }
            hero.LogicInit();
            heroLst.Add(hero);
        }

        CalcRule.blueTeamHero = blueTeamHero;
        CalcRule.redTeamHero = redTeamHero;
    }

    void InitEnv() {
        Transform transMapRoot = GameObject.FindGameObjectWithTag("MapRoot").transform;
        mapRoot = transMapRoot.GetComponent<MapRoot>();
        List<ColliderConfig> envColliCfgLst = GenerateEnvColliCfgs(mapRoot.transEnvCollider);
        logicEnv = new EnvColliders {
            envColliCfgLst = envColliCfgLst
        };
        logicEnv.Init();
    }

    List<ColliderConfig> GenerateEnvColliCfgs(Transform transEnvRoot) {
        List<ColliderConfig> envColliCfgLst = new List<ColliderConfig>();
        BoxCollider[] boxArr = transEnvRoot.GetComponentsInChildren<BoxCollider>();
        for(int i = 0; i < boxArr.Length; i++) {
            Transform trans = boxArr[i].transform;
            var cfg = new ColliderConfig {
                mPos = new PEVector3(trans.position)
            };
            cfg.mSize = new PEVector3(trans.localScale / 2);
            cfg.mType = ColliderType.Box;
            cfg.mAxis = new PEVector3[3];
            cfg.mAxis[0] = new PEVector3(trans.right);
            cfg.mAxis[1] = new PEVector3(trans.up);
            cfg.mAxis[2] = new PEVector3(trans.forward);

            envColliCfgLst.Add(cfg);
        }

        CapsuleCollider[] cylindderArr = transEnvRoot.GetComponentsInChildren<CapsuleCollider>();
        for(int i = 0; i < cylindderArr.Length; i++) {
            Transform trans = cylindderArr[i].transform;
            var cfg = new ColliderConfig {
                mPos = new PEVector3(trans.position)
            };
            cfg.mType = ColliderType.Cylinder;
            cfg.mRadius = (PEInt)(trans.localScale.x / 2);

            envColliCfgLst.Add(cfg);
        }
        return envColliCfgLst;
    }

    private void Update() {
        if(transFollow != null) {
            mapRoot.transCameraRoot.position = transFollow.position;
        }
    }

    public void Tick() {
        //bullet tick
        for(int i = bulletLst.Count - 1; i >= 0; --i) {
            if(bulletLst[i].unitState == SubUnitState.None) {
                bulletLst[i].LogicUnInit();
                bulletLst.RemoveAt(i);
            }
            else {
                bulletLst[i].LogicTick();
            }
        }

        //hero tick
        for(int i = 0; i < heroLst.Count; i++) {
            heroLst[i].LogicTick();
        }
    }

    public void InitCamFollowTrans(int posIndex) {
        transFollow = heroLst[posIndex].mainViewUnit.transform;
    }

    public void AddBullet(Bullet bullet) {
        bulletLst.Add(bullet);
    }

    public void InputKey(List<OpKey> keyLst) {
        for(int i = 0; i < keyLst.Count; i++) {
            OpKey key = keyLst[i];
            MainLogicUnit hero = heroLst[key.opIndex];
            hero.InputKey(key);
        }
    }

    public MainLogicUnit GetSelfHero(int posIndex) {
        return heroLst[posIndex];
    }

    public List<PEColliderBase> GetEnvColliders() {
        return logicEnv.GetEnvColliders();
    }
    public bool CanReleaseSkill(int posIndex, int skillID) {
        return heroLst[posIndex].CanReleaseSkill(skillID);
    }
    public bool IsForbidReleaseSkill(int posIndex) {
        return heroLst[posIndex].IsForbidReleaseSkill();
    }
    public bool CanMove(int posIndex) {
        return heroLst[posIndex].CanMove();
    }
}
