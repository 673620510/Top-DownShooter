using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Settings")]
    public bool friendlyFire;//友军伤害
    private void Awake()
    {
        instance = this;
    }
}
