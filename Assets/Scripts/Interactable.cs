using UnityEngine;
/// <summary>
/// 可交互类
/// </summary>
public class Interactable : MonoBehaviour
{
    protected MeshRenderer mesh;//网格
    [SerializeField]
    private Material highlightMaterial;//高亮材质
    protected Material defaultMaterial;//材质

    private void Start()
    {
        if (mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.material;
    }
    /// <summary>
    /// 更新网格和材质
    /// </summary>
    /// <param name="newMesh"></param>
    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }
    /// <summary>
    /// 交互行为
    /// </summary>
    public virtual void Interaction()
    {
        Debug.Log("Interacted with" + gameObject.name);
    }
    /// <summary>
    /// 高亮物体
    /// </summary>
    /// <param name="active"></param>
    public void HighlightActive(bool active)
    {
        if (active)
        {
            mesh.material = highlightMaterial;
        }
        else
        {
            mesh.material = defaultMaterial;
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.GetInteractables().Add(this);
        playerInteraction.UpdateClosestInteractable();
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.GetInteractables().Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
