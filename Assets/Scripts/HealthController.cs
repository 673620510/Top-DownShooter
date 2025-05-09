using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void ReduceHealth()
    {
        currentHealth--;
    }

    public virtual void IncreaseHealth()
    {
        currentHealth++;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public bool ShouldDie() => currentHealth <= 0;
}
