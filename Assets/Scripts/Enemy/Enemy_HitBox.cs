using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Enemy_HitBox : HitBox
{
    private Enemy enemy;
    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponentInParent<Enemy>();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        enemy.GetHit(damage);
    }
}
