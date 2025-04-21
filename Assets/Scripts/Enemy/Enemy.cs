using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//****************************************
//创建人：逸龙
//功能说明：敌人类
//****************************************
public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int healthPoints = 20;//生命值

    [Header("Idle data 待机数据")]
    public float idleTime;//待机时间
    public float aggresionRange;//侵略范围

    [Header("Move data 移动数据")]
    public float walkSpeed = 1.5f;//移动速度
    public float runSpeed = 3;//追击速度\
    public float turnSpeed;//转向速度
    private bool manualMovement;//手动移动
    private bool manualRotation;//手动转向

    [SerializeField]
    private Transform[] patrolPoints;//巡逻点
    private Vector3[] patrolPointsPosition;//巡逻点数组
    private int currentpatrolIndex;//当前巡逻点

    public bool inBattleMode { get; private set; }//是否处于战斗状态
    public Transform player { get; private set; }
    public Animator anim { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }
    public Enemy_Visuals visuals { get; private set; }//敌人视觉效果类

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        visuals = GetComponent<Enemy_Visuals>();//获取敌人视觉效果类
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoints();
    }

    protected virtual void Update()
    {
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }
    protected virtual void InitializePerk() { }
    /// <summary>
    /// 面向目标
    /// </summary>
    /// <param name="target"></param>
    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngels = transform.rotation.eulerAngles;
        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
    }
    /// <summary>
    /// 是否应进入战斗模式
    /// </summary>
    /// <returns></returns>
    protected bool ShouldEnterBattleMode()
    {
        if (IsPlayerInAgrresionRange() && !inBattleMode)
        {
            EnterBattleMode();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 进入战斗模式
    /// </summary>
    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    public virtual void GetHit()
    {
        EnterBattleMode();
        healthPoints--;
    }
    /// <summary>
    /// 死亡影响
    /// </summary>
    /// <param name="force"></param>
    /// <param name="hitPoint"></param>
    /// <param name="rb"></param>
    public virtual void DeathImpact(Vector3 force,Vector3 hitPoint,Rigidbody rb)
    {
        StartCoroutine(DeathImpactCourutine(force, hitPoint, rb));
    }
    /// <summary>
    /// 死亡影响携程
    /// </summary>
    /// <param name="force"></param>
    /// <param name="hitPoint"></param>
    /// <param name="rb"></param>
    /// <returns></returns>
    private IEnumerator DeathImpactCourutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }
    #region Animation events 动画事件
    /// <summary>
    /// 开启手动移动
    /// </summary>
    /// <param name="manualMovement"></param>
    public void ActivateManualMovement(bool manualMovement) => this.manualMovement = manualMovement;
    /// <summary>
    /// 开启手动转向
    /// </summary>
    /// <param name="manualRotation"></param>
    public void ActivateManualRotation(bool manualRotation) => this.manualRotation = manualRotation;
    /// <summary>
    /// 获取手动移动状态
    /// </summary>
    /// <returns></returns>
    public bool ManualMovementActive() => manualMovement;
    /// <summary>
    /// 获取手动转向状态
    /// </summary>
    /// <returns></returns>
    public bool ManualRotationActive() => manualRotation;
    /// <summary>
    /// 动画触发器
    /// </summary>
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    /// <summary>
    /// 特殊能力触发器
    /// </summary>
    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }
    #endregion
    #region patrol logic 巡逻逻辑
    /// <summary>
    /// 获取巡逻目标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPosition[currentpatrolIndex];

        currentpatrolIndex++;

        if (currentpatrolIndex >= patrolPoints.Length) currentpatrolIndex = 0;

        return destination;
    }
    /// <summary>
    /// 初始化巡逻点
    /// </summary>
    private void InitializePatrolPoints()
    {
        patrolPointsPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }
    #endregion
    public bool IsPlayerInAgrresionRange() => Vector3.Distance(transform.position, player.position) < aggresionRange;
}