/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/09 22:05
	功能: 登录窗口

    //=================*=================\\
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using HOKProtocol;
using PEUtils;
using UnityEngine.UI;

public class LoginWnd : WindowRoot {
    public InputField iptAcct;
    public InputField iptPass;
    public Toggle togSrv;

    public Image imgPrgLoop;
    public Image imgPrgAll;
    public Text txtTime;

    protected override void InitWnd() {
        base.InitWnd();

        System.Random rd = new System.Random();
        iptAcct.text = rd.Next(100, 999).ToString();
        iptPass.text = rd.Next(100, 999).ToString();
    }

    public void ClickLoginBtn() {
        audioSvc.PlayUIAudio("loginBtnClick");
        if(iptAcct.text.Length >= 3 && iptPass.text.Length >= 3) {
            HOKMsg msg = new HOKMsg {
                cmd = CMD.ReqLogin,
                reqLogin = new ReqLogin {
                    acct = iptAcct.text,
                    pass = iptPass.text
                }
            };
            netSvc.SendMsg(msg, (bool result) => {
                if(result == false) {
                    netSvc.InitSvc();
                }
            });
        }
        else {
            root.ShowTips("账号或密码为空");
        }
    }

    public void ClickGMBattleBtn() {
        SetWndState(false);
        GMSystem.Instance.StartSimulate();
    }

    MonoTimer testTimer;
    public void ClickTestBtn() {
        SetText(txtTime, 5);
        testTimer?.DisableTimer();
        testTimer = CreateMonoTimer(
            (loopCount) => {
                this.ColorLog(LogColor.Green, "Loop:" + loopCount);
                SetText(txtTime, 5 - loopCount);
            },
            1000,
            5,
            (isDelay, loopPrg, allPrg) => {
                SetActive(imgPrgLoop);
                if(isDelay) {
                    SetActive(txtTime, false);
                }
                else {
                    SetActive(txtTime);
                }
                imgPrgLoop.fillAmount = 1 - loopPrg;
                imgPrgAll.fillAmount = allPrg;
            },
            () => {
                SetActive(imgPrgLoop, false);
                imgPrgAll.fillAmount = 0;
                this.ColorLog(LogColor.Green, "Loop End");
            },
            3000
            );
    }
}
