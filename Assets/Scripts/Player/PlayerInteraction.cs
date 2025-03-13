using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҽ�����
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();//�ɽ����б�

    private Interactable closestInteractable;//����Ŀɽ�������

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += context => InteractWithClosest();
    }

    /// <summary>
    /// ������Ŀɽ������󻥶�
    /// </summary>
    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateClosestInteractable();
    }
    /// <summary>
    /// ��������Ŀɽ�������
    /// </summary>
    public void UpdateClosestInteractable()
    {
        closestInteractable?.HighlightActive(false);

        closestInteractable = null;
        float closestDistance = float.MaxValue;

        foreach (Interactable interactable in interactables)
        {
            float distance = Vector3.Distance(transform.position, interactable.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = interactable;
            }
        }

        closestInteractable?.HighlightActive(true);
    }
    public List<Interactable> GetInteractables() => interactables;
}
