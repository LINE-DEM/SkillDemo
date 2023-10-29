/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 主要逻辑单位属性状态处理

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using System;

public partial class MainLogicUnit {
    public Action<int, JumpUpdateInfo> OnHPChange;
    /// <summary>
    /// 状态改变回调
    /// </summary>
    public Action<StateEnum, bool> OnStateChange;
    /// <summary>
    /// 受到伤害回调
    /// </summary>
    public Action OnHurt;
    /// <summary>
    /// 死亡时
    /// </summary>
    public Action<MainLogicUnit> OnDeath;
    /// <summary>
    /// 被减速时
    /// </summary>
    public Action<JumpUpdateInfo> OnSlowDown;

    #region 属性状态数据
    private PEInt hp;
    public PEInt Hp {
        private set {
            hp = value;
        }
        get {
            return hp;
        }
    }
    private PEInt def;
    public PEInt Def {
        private set {
            def = value;
        }
        get {
            return def;
        }
    }
    public PEInt AttackSpeedRateBase;
    private PEInt attackSpeedRate;
    public PEInt AttackSpeedRate {
        private set {
            attackSpeedRate = value;

            Skill skill = GetNormalSkill();
            if(skill != null) {
                skill.skillTime = skill.cfg.skillTime * AttackSpeedRateBase / attackSpeedRate;
                skill.spellTime = skill.cfg.spellTime * AttackSpeedRateBase / attackSpeedRate;
            }
        }
        get {
            return attackSpeedRate;
        }
    }
    //沉默计数：沉默时无法施放技能
    int silenceCount;
    public int SilenceCount {
        get {
            return silenceCount;
        }
        set {
            silenceCount = value;
            if(IsSilenced()) {
                OnStateChange?.Invoke(StateEnum.Silenced, true);
            }
            else {
                OnStateChange?.Invoke(StateEnum.Silenced, false);
            }
        }
    }
    bool IsSilenced() {
        return silenceCount != 0;
    }
    //晕眩计数：无法移动，无法施放技能（包括普攻），可以被水银净化解控
    int stunnedCount;
    public int StunnedCount {
        get {
            return stunnedCount;
        }
        set {
            stunnedCount = value;
            if(IsStunned()) {
                InputFakeMoveKey(PEVector3.zero);
                OnStateChange?.Invoke(StateEnum.Stunned, true);
                //this.Log("stun start");
            }
            else {
                OnStateChange?.Invoke(StateEnum.Stunned, false);
                //this.Log("stun end");
            }
        }
    }
    bool IsStunned() {
        return stunnedCount != 0;
    }
    //击飞计数：无法移动，无法施放技能（包括普攻）,无法被水银净化解控
    int knockupCount;
    public int KnockupCount {
        get {
            return knockupCount;
        }
        set {
            knockupCount = value;
            if(IsKnockup()) {
                InputFakeMoveKey(PEVector3.zero);
                OnStateChange?.Invoke(StateEnum.Knockup, true);
                LogicPos += new PEVector3(0, (PEInt)5, 0);
            }
            else {
                OnStateChange?.Invoke(StateEnum.Knockup, false);
                LogicPos += new PEVector3(0, (PEInt)(-5), 0);
            }
        }
    }
    bool IsKnockup() {
        return knockupCount != 0;
    }
    #endregion

    void InitProperties() {
        Hp = ud.unitCfg.hp;
        Def = ud.unitCfg.def;
    }

    public void InitAttackSpeedRate(PEInt rate) {
        AttackSpeedRateBase = rate;
        attackSpeedRate = rate;//每秒钟进行多少次攻击
    }

    #region API Functions
    public void GetDamageBySkill(PEInt damage, Skill skill) {
        OnHurt?.Invoke();
        PEInt hurt = damage - Def;
        if(hurt > 0) {
            Hp -= hurt;
            if(Hp <= 0) {
                Hp = 0;
                unitState = UnitStateEnum.Dead;//状态切换
                InputFakeMoveKey(PEVector3.zero);
                OnDeath?.Invoke(skill.owner);
                PlayAni("death");
                this.Log($"{unitName} hp=0,Died");
            }
            this.Log($"{unitName} hp={hp.RawInt}");

            JumpUpdateInfo jui = null;
            if(IsPlayerSelf() || skill.owner.IsPlayerSelf()) {
                jui = new JumpUpdateInfo {
                    jumpVal = hurt.RawInt,
                    jumpType = JumpTypeEnum.SkillDamage,
                    jumpAni = JumpAniEnum.LeftCurve
                };
            }
            OnHPChange?.Invoke(Hp.RawInt, jui);
        }
    }
    public void GetDamageByBuff(PEInt damage, Buff buff, bool calcCB = true) {
        if(calcCB) {
            OnHurt?.Invoke();
        }
        if(buff.cfg.hitTickAudio != null) {
            PlayAudio(buff.cfg.hitTickAudio);
        }

        PEInt hurt = damage - Def;
        if(hurt > 0) {
            Hp -= hurt;
            if(Hp <= 0) {
                Hp = 0;
                unitState = UnitStateEnum.Dead;//状态切换
                InputFakeMoveKey(PEVector3.zero);
                OnDeath?.Invoke(buff.source);
                PlayAni("death");
            }

            JumpUpdateInfo jui = null;
            if(IsPlayerSelf() || buff.source.IsPlayerSelf() || buff.owner.IsPlayerSelf()) {
                jui = new JumpUpdateInfo {
                    jumpVal = hurt.RawInt,
                    jumpType = JumpTypeEnum.BuffDamage,
                    jumpAni = JumpAniEnum.RightCurve
                };
            }
            OnHPChange?.Invoke(Hp.RawInt, jui);
        }
    }
    public void GetCureByBuff(PEInt cure, Buff buff) {
        if(Hp >= ud.unitCfg.hp) {
            //this.Log("血量已经恢复，治疗溢出");
            return;
        }
        Hp += cure;
        PEInt trueCure = cure;
        if(Hp > ud.unitCfg.hp) {
            trueCure -= (Hp - ud.unitCfg.hp);
            Hp = ud.unitCfg.hp;
        }

        JumpUpdateInfo jui = null;
        //作用目标是英雄角色自己
        //buff来源是英雄自己
        //buff附着目标是英雄角色自己
        if(IsPlayerSelf() || buff.source.IsPlayerSelf() || buff.owner.IsPlayerSelf()) {
            jui = new JumpUpdateInfo {
                jumpVal = trueCure.RawInt,
                jumpType = JumpTypeEnum.Cure,
                jumpAni = JumpAniEnum.CenterUp
            };
        }
        OnHPChange?.Invoke(Hp.RawInt, jui);
    }
    public void ModifyMoveSpeed(PEInt value, Buff buff, bool jumpInfo) {
        //this.ColorLog(PEUtils.LogColor.Green, "移速Offset ScaleValue:" + value.ScaledValue);
        LogicMoveSpeed += value;
        //this.ColorLog(PEUtils.LogColor.Green, "MoveSpeed:" + LogicMoveSpeed.ScaledValue);
        if(value < 0 && jumpInfo) {
            //减速跳字
            JumpUpdateInfo jui = null;
            if(IsPlayerSelf()) {
                jui = new JumpUpdateInfo {
                    jumpType = JumpTypeEnum.SlowSpeed,
                    jumpAni = JumpAniEnum.CenterUp
                };
            }
            OnSlowDown?.Invoke(jui);
        }
    }

    public void ModifyAttackSpeed(PEInt value) {
        AttackSpeedRate += value;
    }
    #endregion

    public bool IsTeam(TeamEnum teamEnum) {
        return ud.teamEnum == teamEnum;
    }
}