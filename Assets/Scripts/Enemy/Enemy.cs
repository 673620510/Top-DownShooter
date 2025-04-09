using UnityEngine;
using UnityEngine.AI;

//****************************************
//创建人：逸龙
//功能说明：敌人类
//****************************************
public class Enemy : MonoBehaviour
{
    public float turnSpeed;//转向速度
    public float aggresionRange;//侵略范围

    [Header("Idle data 待机数据")]
    public float idleTime;

    [Header("Move data 移动数据")]
    public float moveSpeed;
    public float chaseSpeed;

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
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggresionRange);
    }
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
