using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：任务管理器
//****************************************
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public Mission currentMission;//当前任务

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Invoke(nameof(StartMission), 2f);
    }

    private void Update()
    {
        currentMission?.UpdateMission();
    }
    private void StartMission() => currentMission.StartMission();
    public bool MissionCompleted() => currentMission.MissionCompleted();
}
