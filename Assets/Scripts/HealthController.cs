using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    private bool isDead;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ReduceHealth(int damage)
    {
        currentHealth -= damage;
    }

    public virtual void IncreaseHealth()
    {
        currentHealth++;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public bool ShouldDie()
    {
        if (isDead) return false;

        if (currentHealth < 0)
        {
            isDead = true;
            return true;
        }
        return false;
    }
}
