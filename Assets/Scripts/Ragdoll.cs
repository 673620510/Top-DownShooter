using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：布娃娃类
//****************************************
public class Ragdoll : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollParent;//布娃娃父物体

    private Collider[] ragdollColliders;//布娃娃碰撞器
    private Rigidbody[] ragdollRigidbodies;//布娃娃刚体

    private void Awake()
    {
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        RagdollActive(false);
    }
    /// <summary>
    /// 激活布娃娃效果
    /// </summary>
    /// <param name="active"></param>
    public void RagdollActive(bool active)
    {
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = !active;
        }
    }
    /// <summary>
    /// 激活碰撞器
    /// </summary>
    /// <param name="active"></param>
    public void CollidersActive(bool active)
    {
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = active;
        }
    }
}
