/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 目标配置

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class TargetCfg {
    public TargetTeamEnum targetTeam;
    public SelectRuleEnum selectRule;
    public UnitTypeEnum[] targetTypeArr;//可以是多类目标

    //--------辅助参数--------//
    /// <summary>
    /// 查找目标范围距离
    /// </summary>
    public float selectRange;
    /// <summary>
    /// 移动攻击搜索距离，单位：米
    /// </summary>
    public float searchDis;
}

public enum SelectRuleEnum {
    None,
    //单个目标选择规则
    MinHPValue,//最少总血量
    MinHPPercent,//最少百分比血量
    TargetClosestSingle,//靠近目标角色的单个选择
    PositionClosestSingle,//靠近某个位置的单个选择

    //多个目标选择规则
    TargetClosetMultiple,//靠近目标角色的多个选择（范围选择）
    PositionClosestMultiple,//靠近某个位置的多个选择（范围选择）

    Hero,//所有英雄单位
}

/// <summary>
/// 目标队伍
/// </summary>
public enum TargetTeamEnum {
    Dynamic,//用于动态选择目标，通常是方向指向或位置指向技能，在施法成功后通过buff选择目标
    Friend,
    Enemy
}
