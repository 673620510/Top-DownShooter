using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：任务类
//****************************************
public abstract class Mission : ScriptableObject
{
    public string missionName;//任务名称
    [TextArea]
    public string missionDescription;//任务描述

    public abstract void StartMission();//开始任务
    public abstract bool MissionCompleted();//任务完成条件

    public virtual void UpdateMission() { }//更新任务状态
}
