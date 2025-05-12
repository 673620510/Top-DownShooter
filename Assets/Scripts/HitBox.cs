using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class HitBox : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected float damageMultiplier = 1;//伤害倍数
    protected virtual void Awake()
    {
        
    }
    public virtual void TakeDamage(int damage)
    {
    }
}