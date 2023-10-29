/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 战斗计算规则

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using PEUtils;
using System.Collections.Generic;

public static class CalcRule {
    public static Hero[] blueTeamHero;
    public static Hero[] redTeamHero;
    public static Tower[] blueTeamTower;
    public static Tower[] redTeamTower;
    public static List<Soldier> blueTeamSoldier = new List<Soldier>();
    public static List<Soldier> redTeamSoldier = new List<Soldier>();

    public static MainLogicUnit FindMinDisEnemyTarget(MainLogicUnit self, TargetCfg cfg) {
        MainLogicUnit target = null;
        List<MainLogicUnit> targetTeam = GetTargetTeam(self, cfg);

        int count = targetTeam.Count;
        PEVector3 selfPos = self.LogicPos;
        PEInt len = 0;
        for(int i = 0; i < count; i++) {
            PEInt sumRaius = targetTeam[i].ud.unitCfg.colliCfg.mRadius + self.ud.unitCfg.colliCfg.mRadius;
            PEInt tempLen = (targetTeam[i].LogicPos - selfPos).magnitude - sumRaius;
            if(len == 0 || tempLen < len) {
                len = tempLen;
                target = targetTeam[i];
            }
        }
        return target;
    }

    #region 多个目标查找
    public static List<MainLogicUnit> FindMulipleTargetByRule(MainLogicUnit self, TargetCfg cfg, PEVector3 pos) {
        List<MainLogicUnit> searchTeam = GetTargetTeam(self, cfg);
        List<MainLogicUnit> targetLst = null;
        switch(cfg.selectRule) {
            case SelectRuleEnum.TargetClosetMultiple:
                targetLst = FindRangeDisTargetInTeam(self, searchTeam, (PEInt)cfg.selectRange);
                break;
            case SelectRuleEnum.PositionClosestMultiple:
                targetLst = FindRangeDisTargetInPos(pos, searchTeam, (PEInt)cfg.selectRange);
                break;
            case SelectRuleEnum.Hero:
                //TODO
                targetLst = new List<MainLogicUnit>();
                targetLst.AddRange(searchTeam);
                break;
            default:
                PELog.Warn("select target error,check your target cfg.");
                break;
        }
        return targetLst;
    }

    /// <summary>
    /// 指定列表中，离指定目标角色半径范围的所有目标
    /// </summary>
    static List<MainLogicUnit> FindRangeDisTargetInTeam(MainLogicUnit self, List<MainLogicUnit> targetTeam, PEInt range) {
        if(targetTeam == null || range < 0) {
            return null;
        }

        List<MainLogicUnit> targetLst = new List<MainLogicUnit>();
        int count = targetTeam.Count;
        PEVector3 selfPos = self.LogicPos;
        for(int i = 0; i < count; i++) {
            PEInt sumRaius = targetTeam[i].ud.unitCfg.colliCfg.mRadius + self.ud.unitCfg.colliCfg.mRadius;
            PEInt sqrLen = (targetTeam[i].LogicPos - selfPos).sqrMagnitude;
            if(sqrLen < (range + sumRaius) * (range + sumRaius)) {
                targetLst.Add(targetTeam[i]);
            }
        }
        return targetLst;
    }
    /// <summary>
    /// 指定列表中，离指定目标点位置半径范围的所有目标
    /// </summary>
    static List<MainLogicUnit> FindRangeDisTargetInPos(PEVector3 pos, List<MainLogicUnit> targetTeam, PEInt range) {
        if(targetTeam == null || range < 0) {
            return null;
        }

        List<MainLogicUnit> targetLst = new List<MainLogicUnit>();
        int count = targetTeam.Count;
        for(int i = 0; i < count; i++) {
            PEInt radius = targetTeam[i].ud.unitCfg.colliCfg.mRadius;
            PEInt sqrLen = (targetTeam[i].LogicPos - pos).sqrMagnitude;
            if(sqrLen < (range + radius) * (range + radius)) {
                targetLst.Add(targetTeam[i]);
            }
        }
        return targetLst;
    }

    #endregion

    #region 单个目标查找 
    public static MainLogicUnit FindSingleTargetByRule(MainLogicUnit self, TargetCfg cfg, PEVector3 pos) {
        List<MainLogicUnit> serchTeam = GetTargetTeam(self, cfg);
        switch(cfg.selectRule) {
            case SelectRuleEnum.MinHPValue:
                //TODO
                break;
            case SelectRuleEnum.MinHPPercent:
                //TODO
                break;
            case SelectRuleEnum.TargetClosestSingle:
                return FindMinDisTargetInTeam(self, serchTeam, (PEInt)cfg.selectRange);
            case SelectRuleEnum.PositionClosestSingle:
                //TODO
                break;
            default:
                PELog.Warn("select target error, check you target cfg.");
                break;
        }
        return null;
    }

    static MainLogicUnit FindMinDisTargetInTeam(MainLogicUnit self, List<MainLogicUnit> targetTeam, PEInt range) {
        if(targetTeam == null || range < 0) {
            return null;
        }
        MainLogicUnit target = null;
        int count = targetTeam.Count;
        PEVector3 selfPos = self.LogicPos;
        PEInt len = 0;
        for(int i = 0; i < count; i++) {
            PEInt sumRaius = targetTeam[i].ud.unitCfg.colliCfg.mRadius + self.ud.unitCfg.colliCfg.mRadius;
            PEInt tempLen = (targetTeam[i].LogicPos - selfPos).magnitude - sumRaius;
            if(len == 0 || tempLen < len) {
                len = tempLen;
                target = targetTeam[i];
            }
        }

        if(len < range) {
            return target;
        }
        else {
            return null;
        }
    }
    #endregion

    static List<MainLogicUnit> GetTargetTeam(MainLogicUnit self, TargetCfg cfg) {
        List<MainLogicUnit> targetLst = new List<MainLogicUnit>();
        if(self.IsTeam(TeamEnum.Blue)) {
            if(cfg.targetTeam == TargetTeamEnum.Friend) {
                //blue
                if(ContainTargetType(cfg, UnitTypeEnum.Hero)) {
                    targetLst.AddRange(blueTeamHero);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Tower)) {
                    targetLst.AddRange(blueTeamTower);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Soldier)) {
                    targetLst.AddRange(blueTeamSoldier);
                }
            }
            else if(cfg.targetTeam == TargetTeamEnum.Enemy) {
                //red
                if(ContainTargetType(cfg, UnitTypeEnum.Hero)) {
                    targetLst.AddRange(redTeamHero);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Tower)) {
                    //targetLst.AddRange(redTeamTower);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Soldier)) {
                    //targetLst.AddRange(redTeamSoldier);
                }
            }
            else {
                PEUtils.PELog.Warn("TargetTeamEnum is Unknow.");
            }
        }
        else if(self.IsTeam(TeamEnum.Red)) {
            if(cfg.targetTeam == TargetTeamEnum.Friend) {
                //red
                if(ContainTargetType(cfg, UnitTypeEnum.Hero)) {
                    targetLst.AddRange(redTeamHero);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Tower)) {
                    //targetLst.AddRange(redTeamTower);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Soldier)) {
                    //targetLst.AddRange(redTeamSoldier);
                }
            }
            else if(cfg.targetTeam == TargetTeamEnum.Enemy) {
                //blue
                if(ContainTargetType(cfg, UnitTypeEnum.Hero)) {
                    targetLst.AddRange(blueTeamHero);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Tower)) {
                    //targetLst.AddRange(blueTeamTower);
                }
                if(ContainTargetType(cfg, UnitTypeEnum.Soldier)) {
                    //targetLst.AddRange(blueTeamSoldier);
                }
            }
            else {
                PELog.Warn("TargetTeamEnum is Unknow.");
            }
        }
        else {
            PELog.Warn("Self Hero TeamEnum is Unknow.");
        }

        //过滤掉死亡单位
        for(int i = targetLst.Count - 1; i >= 0; --i) {
            if(targetLst[i].unitState == UnitStateEnum.Dead) {
                targetLst.RemoveAt(i);
            }
        }
        return targetLst;
    }

    public static MainLogicUnit FindMinDisTargetInPos(PEVector3 pos, MainLogicUnit[] targetTeam) {
        if(targetTeam == null) {
            return null;
        }

        MainLogicUnit target = null;
        int count = targetTeam.Length;
        PEInt len = 0;
        for(int i = 0; i < count; i++) {
            PEInt radius = targetTeam[i].ud.unitCfg.colliCfg.mRadius;
            PEInt tempLen = (targetTeam[i].LogicPos - pos).magnitude - radius;
            if(len == 0 || tempLen < len) {
                len = tempLen;
                target = targetTeam[i];
            }
        }
        return target;
    }

    static bool ContainTargetType(TargetCfg cfg, UnitTypeEnum targetType) {
        for(int i = 0; i < cfg.targetTypeArr.Length; i++) {
            if(cfg.targetTypeArr[i] == targetType) {
                return true;
            }
        }
        return false;
    }
}