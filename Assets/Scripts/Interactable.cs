using UnityEngine;
/// <summary>
/// �ɽ�����
/// </summary>
public class Interactable : MonoBehaviour
{
    protected MeshRenderer mesh;//����
    [SerializeField]
    private Material highlightMaterial;//��������
    protected Material defaultMaterial;//����

    private void Start()
    {
        if (mesh == null) mesh = GetComponentInChildren<MeshRenderer>();

        defaultMaterial = mesh.material;
    }
    /// <summary>
    /// ��������Ͳ���
    /// </summary>
    /// <param name="newMesh"></param>
    protected void UpdateMeshAndMaterial(MeshRenderer newMesh)
    {
        mesh = newMesh;
        defaultMaterial = newMesh.sharedMaterial;
    }
    /// <summary>
    /// ������Ϊ
    /// </summary>
    public virtual void Interaction()
    {
        Debug.Log("Interacted with" + gameObject.name);
    }
    /// <summary>
    /// ��������
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
