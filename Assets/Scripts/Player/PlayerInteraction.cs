using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 玩家交互类
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    private List<Interactable> interactables = new List<Interactable>();//可交互列表

    private Interactable closestInteractable;//最近的可交互物体

    private void Start()
    {
        Player player = GetComponent<Player>();
        player.controls.Character.Interaction.performed += context => InteractWithClosest();
    }

    /// <summary>
    /// 与最近的可交互对象互动
    /// </summary>
    private void InteractWithClosest()
    {
        closestInteractable?.Interaction();
        interactables.Remove(closestInteractable);

        UpdateClosestInteractable();
    }
    /// <summary>
    /// 更新最近的可交互物体
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
