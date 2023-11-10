/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 主要表现控制

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEMath;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.Serialization;

/// <summary>
/// 攻速/移速动画变化 
/// 技能动画播放
/// 血条信息显示
/// 小地图显示 
/// </summary>
public abstract class MainViewUnit : ViewUnit {
    public Transform skillRange;
    public float fade;
    //血条定位transform
    public Transform hpRoot;
    public Animation ani;
    public Animation_Controller PlayAbleController;
    [OdinSerialize]
    public Dictionary<string, AnimationClip> Clips = new Dictionary<string, AnimationClip>();
    float aniMoveSpeedBase;
    float aniAttackSpeedBase;

    HPWnd hpWnd;
    PlayWnd playWnd;

    MainLogicUnit mainLogicUnit = null;
    public override void Init(LogicUnit logicUnit) {
        base.Init(logicUnit);
        if (PlayAbleController!=null)
        {
            PlayAbleController.Init();
        }
        
        mainLogicUnit = logicUnit as MainLogicUnit;

        //移速
        aniMoveSpeedBase = mainLogicUnit.LogicMoveSpeed.RawFloat;
        aniAttackSpeedBase = mainLogicUnit.AttackSpeedRate.RawFloat;

        //血条显示
        hpWnd = BattleSys.Instance.hpWnd;
        hpWnd.AddHPItemInfo(mainLogicUnit, hpRoot, mainLogicUnit.Hp.RawInt);

        playWnd = BattleSys.Instance.playWnd;

        mainLogicUnit.OnHPChange += UpdateHP;
        mainLogicUnit.OnStateChange += UpdateState;
        mainLogicUnit.OnSlowDown += UpdateJui;
    }

    protected override void Update() {
        
        if(mainLogicUnit.isDirChanged && !mainLogicUnit.IsSkillSpelling()) {
            if(mainLogicUnit.LogicDir.ConvertViewVector3().Equals(Vector3.zero)) {
                PlayAni("free");
            }
            else {
                //todo 动画补丁
                if (PlayAbleController!=null)
                {
                    PlayAbleController.PlaySingleAniamtion(Clips["walk"],1,false);
                    
                }
                else
                {
                    PlayAni("walk");
                }
                
            }
        }
        
        base.Update();
    }

    private void OnDestroy() {
        mainLogicUnit.OnHPChange -= UpdateHP;
    }

    public virtual void OnDeath(MainLogicUnit unit) { }

    public override void PlayAni(string aniName) {
        
        //todo 动画补丁
        if (PlayAbleController!=null)
        {
            PlayAbleController.PlaySingleAniamtion(Clips[aniName],1,false);
            return;
        }

        
        if(aniName == "atk") {
            aniName = "atk" + Random.Range(1, 3);
        }

        if(aniName.Contains("walk")) {
            float moveRate = mainLogicUnit.LogicMoveSpeed.RawFloat / aniMoveSpeedBase;
            foreach (AnimationState anim in ani)
            {
                if (anim.name.Contains("walk"))
                {
                    print(anim.name);
                }
            }
            ani[aniName].speed = moveRate;
            ani.CrossFade(aniName, fade / moveRate);
        }
        else if(aniName.Contains("atk")) {
            if(ani.IsPlaying(aniName)) {
                ani.Stop();
            }
            float attackRate = mainLogicUnit.AttackSpeedRate.RawFloat / aniAttackSpeedBase;
            ani[aniName].speed = attackRate;
            ani.CrossFade(aniName, fade / attackRate);
        }
        else {
            if(ani == null) {
                this.Log("ani is null");
            }
            ani.CrossFade(aniName, fade);
        }
    }

    void UpdateHP(int hp, JumpUpdateInfo jui) {
        if(jui != null) {
            float scaleRate = 1.0f * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
            jui.pos = screenPos * scaleRate;
        }
        hpWnd.SetHPVal(mainLogicUnit, hp, jui);
    }

    public void UpdateJui(JumpUpdateInfo jui) {
        if(jui != null) {
            float scaleRate = 1.0f * ClientConfig.ScreenStandardHeight / Screen.height;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1, 0));
            jui.pos = screenPos * scaleRate;
        }
        hpWnd.SetJumpUpdateInfo(jui);
    }

    public void UpdateState(StateEnum state, bool show) {
        if(state == StateEnum.Knockup
            || state == StateEnum.Silenced
            || state == StateEnum.Silenced) {
            if(mainLogicUnit.IsPlayerSelf() && show) {
                playWnd.SetForbidState();
            }
        }

        hpWnd.SetStateInfo(mainLogicUnit, state, show);
    }

    public void UpdateSkillRotation(PEVector3 skillRotation) {
        viewTargetDir = skillRotation.ConvertViewVector3();
    }

    public void SetAtkSkillRange(bool state, float range = 2.5f) {
        if(skillRange != null) {
            range += mainLogicUnit.ud.unitCfg.colliCfg.mRadius.RawFloat;
            skillRange.localScale = new Vector3(range / 2.5f, range / 2.5f, 1);
            skillRange.gameObject.SetActive(state);
        }
    }

    public void SetBuffFollower(BuffView buffView) {
        buffView.transform.SetParent(transform);
        buffView.transform.localPosition = Vector3.zero;
        buffView.transform.localScale = Vector3.one;
    }

    public void SetImgInfo(int cdTime) {
        if(mainLogicUnit.IsPlayerSelf()) {
            playWnd.SetImgInfo(cdTime);
        }
    }
}
