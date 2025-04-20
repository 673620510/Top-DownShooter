using UnityEngine;

public class Pickup_Weapon : Interactable
{
    [SerializeField] 
    private Weapon_Data weaponData;//武器数据
    [SerializeField]
    private Weapon weapon;//武器

    [SerializeField]
    private BackupWeaponModel[] models;//武器模型

    private bool oldWeapon;//是否旧武器

    private void Start()
    {
        if (oldWeapon == false) weapon = new Weapon(weaponData);

        SetupGameObject();
    }
    /// <summary>
    /// 设置拾取武器
    /// </summary>
    /// <param name="weapon">武器</param>
    /// <param name="transform">位置</param>
    public void SetupPickupWeapon(Weapon weapon, Transform transform)
    {
        oldWeapon = true;
        this.weapon = weapon;
        weaponData = weapon.weaponData;

        this.transform.position = transform.position + new Vector3 (0f, 0.75f, 0f);
    }
    [ContextMenu("Update Item Model 更新物品模型")]//脚本组件右键菜单中添加拓展功能
    /// <summary>
    /// 设置游戏对象
    /// </summary>
    public void SetupGameObject()
    {
        gameObject.name = "Pickup_Weapon - "+ weaponData.weaponType.ToString();
        SetupWeaponModel();
    }
    /// <summary>
    /// 设置武器模型
    /// </summary>
    private void SetupWeaponModel()
    {
        foreach (BackupWeaponModel model in models)
        {
            model.gameObject.SetActive(false);

            if (model.weaponType == weaponData.weaponType) 
            { 
                model.gameObject.SetActive(true);
                UpdateMeshAndMaterial(model.GetComponent<MeshRenderer>());
            }
        }
    }
    public override void Interaction()
    {
        weaponController.PickupWeapon(weapon);

        ObjectPool.instance.ReturnObject(gameObject);
    }
}
