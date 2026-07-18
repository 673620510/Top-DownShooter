using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：关卡生成器类
//****************************************
public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    private List<Enemy> enemyList; 

    [SerializeField]
    private NavMeshSurface navMeshSurface;//导航网格组件

    [Space]
    [SerializeField]
    private Transform lastLevelPart;//最后一个关卡组件
    [SerializeField]
    private List<Transform> levelParts;//关卡组件列表
    private List<Transform> currentLevelParts;//当前关卡组件列表
    private List<Transform> generatedLevelParts = new List<Transform>();//已生成的关卡组件列表
    [SerializeField]
    private SnapPoint nextSnapPoint;//下一个连接点
    private SnapPoint defaultSnapPoint;//默认连接点

    [Space]
    [SerializeField]
    private float generationCooldown;//生成冷却时间
    private float cooldownTimer;//冷却计时器

    private bool generationOver;//是否生成完毕

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        enemyList = new List<Enemy>();
        defaultSnapPoint = nextSnapPoint;
        InitializedGeneration();
    }
    private void Update()
    {
        if (generationOver) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            if (currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (!generationOver)
            {
                FinishGeneration();
            }
        }
    }
    [ContextMenu("Restart generation")]
    private void InitializedGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);

        DestroyOldLevelPartsAndEnemies();
    }
    /// <summary>
    /// 销毁旧的关卡部件和敌人
    /// </summary>
    private void DestroyOldLevelPartsAndEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }

        foreach (Transform part in generatedLevelParts)
        {
            Destroy(part.gameObject);
        }

        generatedLevelParts = new List<Transform>();
        enemyList = new List<Enemy>();
    }

    /// <summary>
    /// 结束生成
    /// </summary>
    private void FinishGeneration()
    {
        generationOver = true;

        GenerateNextLevelPart();

        navMeshSurface.BuildNavMesh();

        foreach (Enemy enemy in enemyList)
        {
            enemy.transform.parent = null;
            enemy.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 生成下一个关卡部件
    /// </summary>
    [ContextMenu("Create next level part 生成下一个关卡部件")]
    private void GenerateNextLevelPart()
    {
        Transform newPart = null;

        if (generationOver)
        {
            newPart = Instantiate(lastLevelPart);
        }
        else
        {
            newPart = Instantiate(ChooseRandomPart());
        }

        generatedLevelParts.Add(newPart);

        LevelPart levelPartScript = newPart.GetComponent<LevelPart>();
        levelPartScript.SnapAndAlignPartTo(nextSnapPoint);

        if (levelPartScript.IntersectionDetected())
        {
            InitializedGeneration();
            return;
        }

        nextSnapPoint = levelPartScript.GetExitPoint();
        enemyList.AddRange(levelPartScript.MyEnemies());
    }
    /// <summary>
    /// 选择随机的关卡部件
    /// </summary>
    /// <returns></returns>
    private Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform choosenPart = currentLevelParts[randomIndex];
        currentLevelParts.RemoveAt(randomIndex);
        return choosenPart;
    }
    /// <summary>
    /// 获取随机敌人
    /// </summary>
    /// <returns></returns>
    public Enemy GetRandomEnemy()
    {
        if (enemyList.Count == 0) return null;
        int randomIndex = Random.Range(0, enemyList.Count);
        return enemyList[randomIndex];
    }
}
