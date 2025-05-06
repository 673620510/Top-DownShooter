using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class HitBox : MonoBehaviour, IDamagable
{
    protected virtual void Awake()
    {
        // Initialize the hitbox if needed
    }
    public virtual void TakeDamage()
    {
    }
}