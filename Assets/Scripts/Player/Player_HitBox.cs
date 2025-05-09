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

    public override void TakeDamage()
    {
        base.TakeDamage();

        Debug.Log("Player HitBox TakeDamage");
        player.health.ReduceHealth();
    }
}
