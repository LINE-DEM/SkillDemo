/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 基础表现控制

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ViewUnit : SerializedMonoBehaviour {
    //Pos
    public bool IsSyncPos;
    public bool PredictPos;
    public int PredictMaxCount;
    public bool SmoothPos;
    public float viewPosAccer;

    //Dir
    public bool IsSyncDir;
    public bool SmoothDir;
    public float viewDirAccer;
    public float AngleMultiplier;

    public Transform RoationRoot;

    int predictCount;
    protected Vector3 viewTargetPos;
    protected Vector3 viewTargetDir;


    LogicUnit logicUnit = null;

    public virtual void Init(LogicUnit logicUnit) {
        this.logicUnit = logicUnit;
        gameObject.name = logicUnit.unitName + "_" + gameObject.name;

        transform.position = logicUnit.LogicPos.ConvertViewVector3();
        if(RoationRoot == null) {
            RoationRoot = transform;
        }
        RoationRoot.rotation = CalcRotation(logicUnit.LogicDir.ConvertViewVector3());
    }

    protected virtual void Update() {
        if(IsSyncDir) {
            UpdateDirection();
        }

        if(IsSyncPos) {
            UpdatePosition();
        }
    }

    void UpdateDirection() {
        if(logicUnit.isDirChanged) {
            viewTargetDir = GetUnitViewDir();
            logicUnit.isDirChanged = false;
        }
        if(SmoothDir) {
            float threshold = Time.deltaTime * viewDirAccer;
            float angle = Vector3.Angle(RoationRoot.forward, viewTargetDir);
            float angleMult = (angle / 180) * AngleMultiplier * Time.deltaTime;

            if(viewTargetDir != Vector3.zero) {
                Vector3 interDir = Vector3.Lerp(RoationRoot.forward, viewTargetDir, threshold + angleMult);
                RoationRoot.rotation = CalcRotation(interDir);
            }
        }
        else {
            RoationRoot.rotation = CalcRotation(viewTargetDir);
        }
    }

    void UpdatePosition() {
        if(PredictPos) {
            if(logicUnit.isPosChanged) {
                //逻辑有Tick，目标位置更新到最新
                viewTargetPos = logicUnit.LogicPos.ConvertViewVector3();
                logicUnit.isPosChanged = false;
                predictCount = 0;
            }
            else {
                if(predictCount > PredictMaxCount) {
                    return;
                }
                //逻辑未Tick，使用预测计算
                float delta = Time.deltaTime;
                //预测位置 = 逻辑速度*逻辑方向
                var predictPos = delta * logicUnit.LogicMoveSpeed.RawFloat * logicUnit.LogicDir.ConvertViewVector3();
                //新的目标位置 = 表现目标位置+预测位置
                viewTargetPos += predictPos;
                ++predictCount;
            }

            //平滑移动
            if(SmoothPos) {
                transform.position = Vector3.Lerp(transform.position, viewTargetPos, Time.deltaTime * viewPosAccer);
            }
            else {
                transform.position = viewTargetPos;
            }
        }
        else {
            //无平滑无预测，强制每帧刷新逻辑层的位置
            ForcePosSync();
        }
    }

    public void ForcePosSync() {
        transform.position = logicUnit.LogicPos.ConvertViewVector3();
    }

    protected virtual Vector3 GetUnitViewDir() {
        return logicUnit.LogicDir.ConvertViewVector3();
    }

    protected Quaternion CalcRotation(Vector3 targetDir) {
        return Quaternion.FromToRotation(Vector3.forward, targetDir);
    }

    public virtual void PlayAni(string aniName) { }

    public virtual void PlayAudio(string audioName, bool loop = false, int delay = 0) {
        AudioSvc.Instance.PlayEntityAudio(audioName, GetComponent<AudioSource>(), loop, delay);
    }
}
