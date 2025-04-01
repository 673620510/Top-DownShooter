using UnityEngine;

public class Pickup_Weapon : Interactable
{
    private PlayerWeaponController weaponController;
    [SerializeField] 
    private Weapon_Data weaponData;
    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private BackupWeaponModel[] models;

    private bool oldWeapon;

    private void Start()
    {
        if (oldWeapon == false) weapon = new Weapon(weaponData);

        UpdateGameObject();
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
    /// 更新游戏物体
    /// </summary>
    public void UpdateGameObject()
    {
        gameObject.name = "Pickup_Weapon - "+ weaponData.weaponType.ToString();
        UpdateItemModel();
    }
    /// <summary>
    /// 更新物品模型
    /// </summary>
    public void UpdateItemModel()
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
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (weaponController == null)
        {
            weaponController = other.GetComponent<PlayerWeaponController>();
        }
    }
}
