/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/09 21:22
	功能: 资源服务

    //=================*=================\\
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using PEPhysx;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance;

    public void InitSvc() {
        Instance = this;
        this.Log("Init ResSvc Done.");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action<float> loadRate, Action loaded) {
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);

        prgCB = () => {
            float progress = sceneAsync.progress;
            loadRate?.Invoke(progress);
            if(progress == 1) {
                loaded?.Invoke();
                prgCB = null;
                sceneAsync = null;
            }
        };
    }

    private void Update() {
        prgCB?.Invoke();
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false) {
        GameObject prefab = null;
        if(!goDic.TryGetValue(path, out prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if(cache) {
                goDic.Add(path, prefab);
            }
        }

        GameObject go = null;
        if(prefab != null) {
            go = Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false) {
        AudioClip au = null;
        if(!adDic.TryGetValue(path, out au)) {
            au = Resources.Load<AudioClip>(path);
            if(cache) {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false) {
        Sprite sp = null;
        if(!spDic.TryGetValue(path, out sp)) {
            sp = Resources.Load<Sprite>(path);
            if(cache) {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    public Buff CreateBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args) {
        BuffCfg cfg = GetBuffConfigByID(buffID);
        switch(cfg.buffType) {
            case BuffTypeEnum.MoveAttack:
                return new MoveAttackBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.ModifySkillTot:
                return new ModifySkillTotBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_Single:
                return new MoveSpeedBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_DynamicGroup:
                return new MoveSpeedBuff_DynamicGroup(source, owner, skill, buffID, args);
            case BuffTypeEnum.ModifySkill:
                return new CommonModifySkillBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Silense:
                return new SilenseBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.ArthurMark:
                return new ArthurMarkBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HPCure:
                return new HPCureBuff_Single(source, owner, skill, buffID, args);
            case BuffTypeEnum.Knockup_Group:
                return new KnockUpBuff_Group(source, owner, skill, buffID, args);

            case BuffTypeEnum.Damage_DynamicGroup:
                return new DamageBuff_DynamicGroup(source, owner, skill, buffID, args);
            case BuffTypeEnum.TargetFlashMove:
                return new TargetFlashMoveBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.ExecuteDamage:
                return new ExecuteDamageBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Damage_StaticGroup:
                return new DamageBuff_StaticGroup(source, owner, skill, buffID, args);

            case BuffTypeEnum.HouyiPasvAttackSpeed:
                return new HouyiPasvAttackSpeedBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiPasvSkillModify:
                return new HouyiMultipleSkillModifyBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiPasvMultiArrow:
                return new HouyiMultipleArrowBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiActiveSkillModify:
                return new HouyiScatterSkillModifyBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.Scatter:
                return new HouyiScatterArrowBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.HouyiMixedMultiScatter:
                return new HouyiMixedMultiScatterBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveSpeed_StaticGroup:
                return new MoveSpeedBuff_StaticGroup(source, owner, skill, buffID, args);

            case BuffTypeEnum.Stun_Single_DynamicTime:
                return new StunBuff_DynamicTime(source, owner, skill, buffID, args);
            case BuffTypeEnum.HPCureInSkillDone:
                return new HPCureInSkillDone(source, owner, skill, buffID, args);
            case BuffTypeEnum.KillBullet:
                return new KillBulletBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.MoveToUITarget:
                return new MoveToUITargetBuff(source, owner, skill, buffID, args);
            case BuffTypeEnum.jiutongSkillTwo:
                return new jiutongSkillTwo(source, owner, skill, buffID, args);
            case BuffTypeEnum.GroupHitBackByPoint:
                return new GroupHitBackByPoint(source, owner, skill, buffID, args);
            //TOADD
            case BuffTypeEnum.None:
            default:
                this.Error("Create Buff Failed,BuffID:" + buffID);
                return null;
        }
    }

    public Bullet CreateBullet(MainLogicUnit source, MainLogicUnit target, Skill skill) {
        switch(skill.cfg.bulletCfg.bulletType) {
            case BulletTypeEnum.SkillTarget:
                return new TargetBullet(source, target, skill);
            case BulletTypeEnum.UIDirection:
                return new DirectionBullet(source, skill);
            case BulletTypeEnum.UIPosition:
            case BulletTypeEnum.BuffSearch:
            case BulletTypeEnum.UIDirectionNoCollider:
                return new DirectionBullet(source, skill);
            default:
                this.Error("Create Bullet Error.");
                return null;
        }
    }

    public UnitCfg GetUnitConfigByID(int unitID) {
        switch(unitID) {
            case 101:
                return new UnitCfg {
                    unitID = 101,
                    unitName = "亚瑟",
                    resName = "arthur",
                    hitHeight = (PEInt)1.5F,
                    hp = 6500,
                    def = 0,
                    moveSpeed = 5,
                    colliCfg = new ColliderConfig {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.5f
                    },
                    pasvBuff = new int[] { 10100 ,10300},
                    skillArr = new int[] { 1010, 1011, 1012, 1013 }
                };
            case 102:
                return new UnitCfg {
                    unitID = 102,
                    unitName = "酒桶",
                    resName = "Jiutong",
                    hitHeight = (PEInt)1.5F,
                    hp = 3500,
                    def = 10,
                    moveSpeed = 5,
                    colliCfg = new ColliderConfig {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.5f
                    },
                    pasvBuff = new int[] { 10200, 10201 ,10300},
                    skillArr = new int[] { 1020, 1030, 1032, 1033 }
                };
            case 103:
                return new UnitCfg {
                    unitID = 103,
                    unitName = "酒桶",
                    resName = "houyi",
                    hitHeight = (PEInt)1.5F,
                    hp = 3500,
                    def = 10,
                    moveSpeed = 5,
                    colliCfg = new ColliderConfig {
                        mType = ColliderType.Cylinder,
                        mRadius = (PEInt)0.5f
                    },
                    pasvBuff = new int[] { 10200, 10201 ,10300},
                    skillArr = new int[] { 1030, 1021, 1022, 1023 }
                };
        }
        return null;
    }
    public BuffCfg GetBuffConfigByID(int buffID) {
        switch(buffID) {
            case 10100:
                return ResBuffConfigs.buff_10100;
            //Arthur1技能
            case 10110://移速加速
                return ResBuffConfigs.buff_10110;
            case 10111:
                return ResBuffConfigs.buff_10111;
            case 10140:
                return ResBuffConfigs.buff_10140;
            case 10141:
                return ResBuffConfigs.buff_10141;
            case 10142:
                return ResBuffConfigs.buff_10142;
            //Arthur2技能
            case 10120:
                return ResBuffConfigs.buff_10120;
            //Arthur3技能
            case 10130:
                return ResBuffConfigs.buff_10130;
            case 10131:
                return ResBuffConfigs.buff_10131;
            case 10132:
                return ResBuffConfigs.buff_10132;
            case 10133:
                return ResBuffConfigs.buff_10133;
            //Houyi被动技能
            case 10200:
                return ResBuffConfigs.buff_10200;
            case 10201:
                return ResBuffConfigs.buff_10201;
            case 10250:
                return ResBuffConfigs.buff_10250;
            //Houyi1技能
            case 10210://技能替换
                return ResBuffConfigs.buff_10210;
            case 10240://scatter
                return ResBuffConfigs.buff_10240;
            case 10260://mixed
                return ResBuffConfigs.buff_10260;
            //Houyi2技能
            case 10220:
                return ResBuffConfigs.buff_10220;
            case 10221:
                return ResBuffConfigs.buff_10221;
            case 10222:
                return ResBuffConfigs.buff_10222;
            case 10223:
                return ResBuffConfigs.buff_10223;
            //Houyi3技能
            case 10230:
                return ResBuffConfigs.buff_10230;
            case 10231:
                return ResBuffConfigs.buff_10231;
            //酒桶被动
            case 10300:
                return ResBuffConfigs.buff_10300;
            case 10301:
                return ResBuffConfigs.buff_10301;
            case 10302:
                return ResBuffConfigs.buff_10302;
            case 10303:
                return ResBuffConfigs.buff_10303;
            case 10304:
                return ResBuffConfigs.buff_10304;
            case 10305:
                return ResBuffConfigs.buff_10305;
            case 10306:
                return ResBuffConfigs.buff_10306;
            case 10307:
                return ResBuffConfigs.buff_10307;
            case 10308:
                return ResBuffConfigs.buff_10308;
            case 10309:
                return ResBuffConfigs.buff_10309;
            //通用
            case 90000:
                return ResBuffConfigs.buff_90000;
            default:
                break;
        }
        this.Error("Get Buff Config Failed,buffID:" + buffID);
        return null;
    }
    public SkillCfg GetSkillConfigByID(int skillID) {
        switch(skillID) {
            case 1010:
                return ResSkillConfigs.sk_1010;
            case 1011:
                return ResSkillConfigs.sk_1011;
            case 1012:
                return ResSkillConfigs.sk_1012;
            case 1013:
                return ResSkillConfigs.sk_1013;
            case 1014:
                return ResSkillConfigs.sk_1014;
            case 1020:
                return ResSkillConfigs.sk_1020;
            case 1021:
                return ResSkillConfigs.sk_1021;
            case 1022:
                return ResSkillConfigs.sk_1022;
            case 1023:
                return ResSkillConfigs.sk_1023;
            case 1024:
                return ResSkillConfigs.sk_1024;
            case 1025:
                return ResSkillConfigs.sk_1025;
            case 1026:
                return ResSkillConfigs.sk_1026;
            case 1030:
                return ResSkillConfigs.sk_1030;
            case 1031:
                return ResSkillConfigs.sk_1031;
            case 1032:
                return ResSkillConfigs.sk_1032;
            case 1033:
                return ResSkillConfigs.sk_1033;
            
            default:
                this.Error("Get Skill Config Failed,skillID:" + skillID);
                return null;
        }
    }
    public MapCfg GetMapConfigByID(int mapID) {
        switch(mapID) {
            case 101:
                return new MapCfg {
                    mapID = 101,
                    blueBorn = new PEVector3(-27, 0, 0),
                    redBorn = new PEVector3(27, 0, 0),
                    bornDelay = 15000,
                    bornInterval = 2000,
                    waveInterval = 50000
                };
            case 102:
                return new MapCfg {
                    mapID = 102,
                    blueBorn = new PEVector3(-5, 0, -3),
                    redBorn = new PEVector3(5, 0, -3),
                    bornDelay = 15000,
                    bornInterval = 2000,
                    waveInterval = 50000
                };
            default:
                return null;
        }
    }
}
