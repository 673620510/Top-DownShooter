using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ҽ�����
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactables;//�ɽ����б�

    private Interactable closestInteractable;//����Ŀɽ�������

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
}
