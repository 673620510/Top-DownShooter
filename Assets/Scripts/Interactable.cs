using UnityEngine;
/// <summary>
/// 可交互类
/// </summary>
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mesh;//网格
    [SerializeField]
    private Material highlightMaterial;//高亮材质
    private Material defaultMaterial;//材质

    private void Start()
    {
        if (mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.material;
    }
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
    private void OnTriggerEnter(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.interactables.Add(this);
        playerInteraction.UpdateClosestInteractable();
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();

        if (playerInteraction == null) return;

        playerInteraction.interactables.Remove(this);
        playerInteraction.UpdateClosestInteractable();
    }
}
