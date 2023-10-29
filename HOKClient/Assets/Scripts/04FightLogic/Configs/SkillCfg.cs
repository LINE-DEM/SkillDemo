/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 技能配置

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class SkillCfg {
    public int skillID;
    /// <summary>
    /// 技能图标
    /// </summary>
    public string iconName;
    /// <summary>
    /// 施法动画名称
    /// </summary>
    public string aniName;
    /// <summary>
    /// 施放方式
    /// </summary>
    public ReleaseModeEnum releaseMode;
    /// <summary>
    /// 目标选择配置
    /// </summary>
    public TargetCfg targetCfg;
    /// <summary>
    /// 弹道配置
    /// </summary>
    public BulletCfg bulletCfg;
    /// <summary>
    /// CD时间：ms
    /// </summary>
    public int cdTime;
    /// <summary>
    /// 施法时间（前摇）：ms
    /// </summary>
    public int spellTime;
    /// <summary>
    /// 是否为普通攻击
    /// </summary>
    public bool isNormalAttack;
    /// <summary>
    /// 技能全长时间，包含前摇，后摇
    /// 后摇动作均可被移动中断，但技能总时间不能变短
    /// </summary>
    public int skillTime;
    /// <summary>
    /// 基础伤害数值
    /// </summary>
    public int damage;
    /// <summary>
    /// 附加Buff
    /// </summary>
    public int[] buffIDArr;

    //音效相关
    public string audio_start;//施法开始
    public string audio_work;//施法成功
    public string audio_hit;//施法命中
}

public enum ReleaseModeEnum {
    None,
    Click,//点击施放
    Postion,//位置施放
    Direction//方向施放
}
