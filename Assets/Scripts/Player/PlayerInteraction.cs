using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家交互类
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    public List<Interactable> interactables;//可交互列表

    private Interactable closestInteractable;//最近的可交互物体

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
