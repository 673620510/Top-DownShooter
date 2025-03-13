using UnityEngine;

public class Pickup_Ammo : Interactable
{
    public override void Interaction()
    {
        base.Interaction();

        Debug.Log("Added AMMO to Weapon");
    }
    
}
