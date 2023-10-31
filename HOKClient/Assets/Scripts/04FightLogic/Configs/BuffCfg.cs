/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: Buff配置

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class BuffCfg {
    public int buffID;
    public string buffName;
    /// <summary>
    /// buff类型，用来创建不同类型的buff
    /// </summary>
    public BuffTypeEnum buffType;
    /// <summary>
    /// buff附着目标
    /// </summary>
    public AttachTypeEnum attacher;
    /// <summary>
    /// buff作用目标，为null默认影响附着对象
    /// </summary>
    public TargetCfg impacter;

    public int buffDelay;
    public int buffInterval;
    public int buffDuration;//（不包含delay）0：生效1次，-1：永久生效
    public StaticPosTypeEnum staticPosType;

    public string buffAudio;
    public string buffEffect;
    public string hitTickAudio;
}

public enum AttachTypeEnum {
    None,
    Caster,//Arthur的1技能加速buff
    Target,//Arthur的1技能沉默buff

    Indie,//Arthur大招产生的持续范围伤害

    Bullet,//Houyi大招命中目标时产生的范围伤害
}

public enum StaticPosTypeEnum {
    None,
    SkillCasterPos,//Buff所属技能施放者的位置
    SkillLockTargetPos,//Buff所属技能锁定目标的位置
    BulletHitTargetPos,//子弹命中目标的位置
    UIInputPos,//UI输入位置信息
    BulletPos,//子弹的位置
}

public enum BuffTypeEnum {
    None,
    HPCure,//治疗
    ModifySkillTot,
    ModifySkill,
    MoveSpeed_Single,//单体加速buff
    ArthurMark,//Arthur1技能的标记伤害Buff
    Silense,//沉默
    TargetFlashMove,
    DirectionFlashMove,//TODO
    ExecuteDamage,
    Knockup_Group,//群体击飞

    Stun_Single_DynamicTime,

    //houyi专区buff
    HouyiActiveSkillModify,//Houyi主动技能修改buff
    Scatter,

    HouyiPasvAttackSpeed,//Houyi被动攻速加成buff
    HouyiPasvSkillModify,//Houyi被动技能修改Buff
    HouyiPasvMultiArrow,//Houyi被动技能多重射击Buff
    HouyiMixedMultiScatter,//混合多重射击与散射


    MoveSpeed_DynamicGroup,//动态群体移速Buff
    MoveSpeed_StaticGroup,//静态群体移速buff
    Damage_DynamicGroup,//动态群体伤害
    Damage_StaticGroup,
    MoveAttack,//移动攻击
    
    //酒桶专区
    HPCureInSkillDone,//释放技能后百分比回血
    KillBullet,
    MoveToUITarget,
    jiutongSkillTwo,
}
