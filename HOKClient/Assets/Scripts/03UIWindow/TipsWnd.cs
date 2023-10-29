/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2021/02/25 15:16
	功能: Tips弹窗

    //=================*=================\\
           教学官网：www.qiqiker.com
           Plane老师微信: PlaneZhong
           关注微信服务号: qiqikertuts           
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TipsWnd : WindowRoot {
    public Image bgTips;
    public Text txtTips;
    public Animator ani;

    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();

    protected override void InitWnd() {
        base.InitWnd();
        SetActive(bgTips, false);
        tipsQue.Clear();
    }

    private void Update() {
        if(tipsQue.Count > 0 && isTipsShow == false) {
            string tips = tipsQue.Dequeue();
            isTipsShow = true;
            SetTips(tips);
        }
    }

    private void SetTips(string tips) {
        int len = tips.Length;
        SetActive(bgTips);
        txtTips.text = tips;
        bgTips.GetComponent<RectTransform>().sizeDelta = new Vector2(35 * len + 100, 80);

        ani.Play("TipsWnd", 0, 0);
    }

    public void AddTips(string tips) {
        tipsQue.Enqueue(tips);
    }

    public void AniPlayDone() {
        SetActive(bgTips, false);
        isTipsShow = false;
    }
}
