using UnityEngine;
using UnityEngine.AI;

//****************************************
//创建人：逸龙
//功能说明：敌人类
//****************************************
public class Enemy : MonoBehaviour
{
    [Header("Idle data 待机数据")]
    public float idleTime;
    public float aggresionRange;//侵略范围

    [Header("Move data 移动数据")]
    public float moveSpeed;
    public float chaseSpeed;
    public float turnSpeed;//转向速度
    private bool manualMovement;//手动移动
    private bool manualRotation;//手动转向

    [SerializeField]
    private Transform[] patrolPoint;//巡逻点
    private int currentpatrolIndex;//当前巡逻点
    
    public Transform player { get; private set; }
    public Animator anim { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

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
        
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }
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
    /// 玩家是否在侵略范围内
    /// </summary>
    /// <returns></returns>
    public bool PlayerInAggresionRange() => Vector3.Distance(transform.position, player.position) < aggresionRange;
    /// <summary>
    /// 获取巡逻目标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoint[currentpatrolIndex].transform.position;

        currentpatrolIndex++;

        if (currentpatrolIndex >= patrolPoint.Length) currentpatrolIndex = 0;

        return destination;
    }
    /// <summary>
    /// 初始化巡逻点
    /// </summary>
    private void InitializePatrolPoints()
    {
        foreach (Transform t in patrolPoint)
        {
            t.parent = null;
        }
    }
    /// <summary>
    /// 面向目标
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngels = transform.rotation.eulerAngles;
        float yRotation = Mathf.LerpAngle(currentEulerAngels.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        return Quaternion.Euler(currentEulerAngels.x, yRotation, currentEulerAngels.z);
    }
}
