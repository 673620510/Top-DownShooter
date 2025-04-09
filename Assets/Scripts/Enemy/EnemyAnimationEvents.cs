using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：敌人动画事件类
//****************************************
public class EnemyAnimationEvents : MonoBehaviour
{
    private Enemy enemy;
    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void AnimationTrigger() => enemy.AnimationTrigger();
}
