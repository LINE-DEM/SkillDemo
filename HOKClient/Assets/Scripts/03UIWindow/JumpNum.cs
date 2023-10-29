/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 伤害治疗飘字

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JumpTypeEnum {
    None,
    SkillDamage,
    BuffDamage,
    Cure,
    SlowSpeed,
}

public enum JumpAniEnum {
    None,
    LeftCurve,
    RightCurve,
    CenterUp
}

public class JumpUpdateInfo {
    public int jumpVal;
    public Vector2 pos;
    public JumpTypeEnum jumpType;
    public JumpAniEnum jumpAni;
}

public class JumpNum : MonoBehaviour {
    public RectTransform rect;
    public Animator ani;
    public Text txt;

    public int MaxFont;
    public int MinFont;
    public int MaxFontValue;
    public Color skillDamageColor;
    public Color buffDamageColor;
    public Color cureDamageColor;
    public Color slowSpeedColor;


    JumpNumPool ownerPool;
    public void Init(JumpNumPool ownerPool) {
        this.ownerPool = ownerPool;
    }

    public void Show(JumpUpdateInfo ji) {
        int fontSize = (int)Mathf.Clamp(ji.jumpVal * 1.0f / MaxFontValue, MinFont, MaxFont);
        txt.fontSize = fontSize;
        rect.anchoredPosition = ji.pos;

        switch(ji.jumpType) {
            case JumpTypeEnum.SkillDamage:
                txt.text = ji.jumpVal.ToString();
                txt.color = skillDamageColor;
                break;
            case JumpTypeEnum.BuffDamage:
                txt.text = ji.jumpVal.ToString();
                txt.color = buffDamageColor;
                break;
            case JumpTypeEnum.Cure:
                txt.text = "+" + ji.jumpVal;
                txt.color = cureDamageColor;
                break;
            case JumpTypeEnum.SlowSpeed:
                txt.text = "减速";
                txt.color = slowSpeedColor;
                break;
            case JumpTypeEnum.None:
            default:
                break;
        }

        switch(ji.jumpAni) {
            case JumpAniEnum.LeftCurve:
                ani.Play("JumpLeft", 0);
                break;
            case JumpAniEnum.RightCurve:
                ani.Play("JumpRight", 0);
                break;
            case JumpAniEnum.CenterUp:
                ani.Play("JumpCenter", 0);
                break;
            case JumpAniEnum.None:
            default:
                break;
        }

        StartCoroutine(Recycle());
    }

    IEnumerator Recycle() {
        yield return new WaitForSeconds(0.75f);
        ani.Play("Empty");
        ownerPool.PushOne(this);
    }
}

public class JumpNumPool {
    Transform poolRoot;
    private Queue<JumpNum> jumpNumQue;

    public JumpNumPool(int count, Transform poolRoot) {
        this.poolRoot = poolRoot;
        jumpNumQue = new Queue<JumpNum>();

        for(int i = 0; i < count; i++) {
            PushOne(CreateOne());
        }
    }

    int index = 0;
    int Index {
        get {
            return ++index;
        }
    }
    JumpNum CreateOne() {
        GameObject go = ResSvc.Instance.LoadPrefab("UIPrefab/DynamicItem/JumpNum");
        go.name = "JumpNum_" + Index;
        go.transform.SetParent(poolRoot);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        JumpNum jn = go.GetComponent<JumpNum>();
        jn.Init(this);
        return jn;
    }

    public JumpNum PopOne() {
        if(jumpNumQue.Count > 0) {
            return jumpNumQue.Dequeue();
        }
        else {
            this.Warn("飘字超额，动态调整上限");
            PushOne(CreateOne());
            return PopOne();
        }
    }

    public void PushOne(JumpNum jn) {
        jumpNumQue.Enqueue(jn);
    }
}
