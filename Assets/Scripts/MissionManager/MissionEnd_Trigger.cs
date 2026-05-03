using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class MissionEnd_Trigger : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;

        if (MissionManager.instance.MissionCompleted()) Debug.Log("Level Completed!");
    }
}
