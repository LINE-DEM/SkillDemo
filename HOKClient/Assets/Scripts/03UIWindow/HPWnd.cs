/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 血条及伤害数据显示

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

public class HPWnd : WindowRoot {
    public Transform hpItemRoot;
    public Transform jumpNumRoot;
    public int jumpNumCount;

    private Dictionary<MainLogicUnit, ItemHP> itemDic;
    JumpNumPool pool;
    protected override void InitWnd() {
        base.InitWnd();
        itemDic = new Dictionary<MainLogicUnit, ItemHP>();
        pool = new JumpNumPool(jumpNumCount, jumpNumRoot);
    }

    public void AddHPItemInfo(MainLogicUnit unit, Transform trans, int hp) {
        if(itemDic.ContainsKey(unit)) {
            this.Error(unit.unitName + " hp item is already exist.");
        }
        else {
            //判断单位类型
            string path = GetItemPath(unit.unitType);
            GameObject go = resSvc.LoadPrefab(path, true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            ItemHP ih = go.GetComponent<ItemHP>();
            ih.InitItem(unit, trans, hp);
            itemDic.Add(unit, ih);
        }
    }

    string GetItemPath(UnitTypeEnum unitType) {
        string path = "";
        switch(unitType) {
            case UnitTypeEnum.Hero:
                path = "UIPrefab/DynamicItem/ItemHPHero";
                break;
            case UnitTypeEnum.Soldier:
                path = "UIPrefab/DynamicItem/ItemHPSoldier";
                break;
            case UnitTypeEnum.Tower:
                path = "UIPrefab/DynamicItem/ItemHPTower";
                break;
            default:
                break;
        }
        return path;
    }

    public void SetHPVal(MainLogicUnit key, int hp, JumpUpdateInfo jui) {
        if(itemDic.TryGetValue(key, out ItemHP item)) {
            item.UpdateHPPrg(hp);
        }

        if(jui != null) {
            JumpNum jn = pool.PopOne();
            if(jn != null) {
                jn.Show(jui);
            }
        }
    }

    public void SetJumpUpdateInfo(JumpUpdateInfo jui) {
        if(jui != null) {
            JumpNum jn = pool.PopOne();
            if(jn != null) {
                jn.Show(jui);
            }
        }
    }

    public void SetStateInfo(MainLogicUnit key, StateEnum state, bool show = true) {
        if(itemDic.TryGetValue(key, out ItemHP item)) {
            item.SetStateInfo(state, show);
        }
    }

    public void RmvHPItemInfo(MainLogicUnit key) {
        if(itemDic.TryGetValue(key, out ItemHP item)) {
            Destroy(item.gameObject);
            itemDic.Remove(key);
        }
    }


    protected override void UnInitWnd() {
        base.UnInitWnd();
        for(int i = hpItemRoot.childCount; i >= 0; --i) {
            Destroy(hpItemRoot.GetChild(i));
        }
        for(int i = jumpNumRoot.childCount; i >= 0; --i) {
            Destroy(jumpNumRoot.GetChild(i));
        }

        if(itemDic != null) {
            itemDic.Clear();
        }
    }
}
