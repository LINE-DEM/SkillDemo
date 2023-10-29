/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: houyi被动加攻速buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEMath;

public class HouyiPasvAttackSpeedBuffCfg : BuffCfg {
    public int overCount;//叠加层数
    public int speedAddtion;//加成百分比
    public int resetTime;
}

public class HouyiPasvAttackSpeedBuff : Buff {
    int currOverCount;//叠加层数
    int maxOverCount;//最大叠加层数
    int resetTime;

    PEInt speedAddtion;
    PEInt speedOffset;

    public HouyiPasvAttackSpeedBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    public override void LogicInit() {
        base.LogicInit();

        currOverCount = 0;
        HouyiPasvAttackSpeedBuffCfg hpasbc = cfg as HouyiPasvAttackSpeedBuffCfg;
        maxOverCount = hpasbc.overCount;
        resetTime = hpasbc.resetTime;
        speedAddtion = hpasbc.speedAddtion;
        speedOffset = PEInt.zero;

        Skill[] skillArr = source.GetAllSkill();
        for(int i = 0; i < skillArr.Length; i++) {
            
            //if(skillArr[i].cfg.isNormalAttack)
                skillArr[i].SpellSuccCallback += OnSpellSkillSucc;
        }
        
    }

    void OnSpellSkillSucc(Skill skillReleased) {
        if(skillReleased.cfg.isNormalAttack) {
            timeCount = 0;
            if(currOverCount >= maxOverCount) {
                return;
            }
            else {
                ++currOverCount;
                isCounter = true;
                PEInt addition = owner.AttackSpeedRateBase * (speedAddtion / 100);
                speedOffset += addition;
                owner.ModifyAttackSpeed(addition);
            }
        }
        else {
            if(skillReleased.skillID != 1021) {
                ResetSpeed();
            }
        }
    }

    int timeCount;
    bool isCounter;
    protected override void Tick() {
        base.Tick();
        if(isCounter) {
            timeCount += ServerConfig.ServerLogicFrameIntervelMs;
            if(timeCount >= resetTime) {
                ResetSpeed();
                timeCount = 0;
                isCounter = false;
            }
        }
    }

    void ResetSpeed() {
        owner.ModifyAttackSpeed(-speedOffset);
        speedOffset = PEInt.zero;
        currOverCount = 0;
    }
}
