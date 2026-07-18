using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：任务计时器类
//****************************************
[CreateAssetMenu(fileName = "New Key Mission 新钥匙任务", menuName = "Missions/Key Mission 任务/钥匙任务")]
public class Mission_KeyFind : Mission
{
    [SerializeField]
    private GameObject key;//钥匙对象
    private bool keyFound;//钥匙是否被找到
    public override void StartMission()
    {
        MissionObject_Key.OnKeyPickedUp += PickupKey; // 订阅钥匙拾取事件

        Enemy enemy =LevelGenerator.instance.GetRandomEnemy();// 获取随机敌人
        enemy.GetComponent<Enemy_DropController>()?.Givekey(key); // 给敌人分配钥匙
        enemy.MakeEnemyVIP(); // 将敌人设为VIP
    }
    public override bool MissionCompleted()
    {
        return keyFound;
    }
    /// <summary>
    /// 拾取钥匙
    /// </summary>
    private void PickupKey()
    {
        keyFound = true;
        MissionObject_Key.OnKeyPickedUp -= PickupKey; // 取消订阅钥匙拾取事件
        Debug.Log("I Picked up a Key! 我拾取了钥匙");
    }
}
