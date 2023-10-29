/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 被动多重射击buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;

public class HouyiMultipleArrowBuffCfg : BuffCfg {
    public int arrowCount;
    public int arrowDelay;
    public float posOffset;
}

public class HouyiMultipleArrowBuff : Buff {
    int arrowCount;
    int arrowDelay;
    PEInt posOffset;

    public HouyiMultipleArrowBuff(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    MainLogicUnit targetHero;
    public override void LogicInit() {
        base.LogicInit();

        HouyiMultipleArrowBuffCfg hpmabc = cfg as HouyiMultipleArrowBuffCfg;
        arrowCount = hpmabc.arrowCount;
        arrowDelay = hpmabc.arrowDelay;
        posOffset = (PEInt)hpmabc.posOffset;

        targetHero = skill.lockTarget;

        for(int i = 0; i < arrowCount; i++) {
            TargetBullet bullet = source.CreateSkillBullet(source, targetHero, skill) as TargetBullet;
            bullet.SetDelayData((i + 1) * arrowDelay);

            if(i % 2 == 0) {
                bullet.SetOffsetPos(PEVector3.up * posOffset);
            }
            else {
                bullet.SetOffsetPos(PEVector3.up * -posOffset);
            }

            bullet.HitTargetCB = (MainLogicUnit target, object[] args) => {
                target.GetDamageByBuff(skill.cfg.damage, this);
            };
        }
    }
}
