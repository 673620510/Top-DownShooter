using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人盾牌类
//****************************************
public class Enemy_Shield : MonoBehaviour
{
    private Enemy_Melee enemy;

    [SerializeField]
    private int durability;//耐久度

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy_Melee>();
    }
    /// <summary>
    /// 减少耐久度
    /// </summary>
    public void ReduceDurability()
    {
        durability--;
        if (durability <= 0)
        {
            //enemy.anim.SetFloat("ChaseIndex", 0);//恢复原来动画
            Destroy(gameObject);
        }
    }
}
