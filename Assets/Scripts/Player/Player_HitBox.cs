using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Player_HitBox : HitBox
{
    Player player;
    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        player.health.ReduceHealth(damage);
    }
}
