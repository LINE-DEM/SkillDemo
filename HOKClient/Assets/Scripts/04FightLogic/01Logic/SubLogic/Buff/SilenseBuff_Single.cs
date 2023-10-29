/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 沉默buff

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

public class SilenseBuff_Single : Buff {
    public SilenseBuff_Single(MainLogicUnit source, MainLogicUnit owner, Skill skill, int buffID, object[] args = null)
        : base(source, owner, skill, buffID, args) {
    }

    protected override void Start() {
        base.Start();

        owner.SilenceCount += 1;
    }

    protected override void End() {
        base.End();
        owner.SilenceCount -= 1;
    }
}
