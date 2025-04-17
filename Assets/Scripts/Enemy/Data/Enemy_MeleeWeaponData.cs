using System.Collections.Generic;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data/Melee Weapon Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<AttackData_EnemyMelee> attackData;
    public float turnSpeed = 10f;//转向速度
}
