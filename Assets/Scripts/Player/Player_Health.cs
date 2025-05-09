using System;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Player_Health : HealthController
{
    private Player player;

    public bool isDead { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();
    }
    public override void ReduceHealth()
    {
        base.ReduceHealth();

        if (ShouldDie())
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        player.anim.enabled = false;
        player.ragdoll.RagdollActive(true);
    }
}
