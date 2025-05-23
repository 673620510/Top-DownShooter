using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：关卡生成器类
//****************************************
public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform lastLevelPart;//最后一个关卡组件
    [SerializeField]
    private List<Transform> levelParts;//关卡组件列表
    private List<Transform> currentLevelParts;//当前关卡组件列表
    private List<Transform> generatedLevelParts = new List<Transform>();//已生成的关卡组件列表
    [SerializeField]
    private SnapPoint nextSnapPoint;//下一个连接点
    private SnapPoint defaultSnapPoint;

    [Space]
    [SerializeField]
    private float generationCooldown;//生成冷却时间
    private float cooldownTimer;//冷却计时器

    private bool generationOver;//是否生成完毕

    private void Start()
    {
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

        DestroyOldLevelParts();
    }

    private void DestroyOldLevelParts()
    {
        foreach (Transform part in generatedLevelParts)
        {
            Destroy(part.gameObject);
        }

        generatedLevelParts = new List<Transform>();
    }

    /// <summary>
    /// 结束生成
    /// </summary>
    private void FinishGeneration()
    {
        generationOver = true;

        GenerateNextLevelPart();
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
}
