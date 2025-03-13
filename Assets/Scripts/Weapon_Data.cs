using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapon System/Weapon Data")]
public class Weapon_Data : ScriptableObject//Unity�ṩ���������ô洢����
{
    public string weaponName;//��������

    [Header("Magazine details ��������")]
    public int bulletsInMagazine;//��ǰǹ֧��ҩ��
    public int magazineCapacity;//����������
    public int totalReserveAmmo;//��ǰǹ֧�ܱ��õ�ҩ��

    [Header("Regular shot �������")]
    public ShootType shootType;//�������
    public int bulletPerShot = 1;//ÿ�ο����ӵ�����
    public float fireRate;//ÿ������

    [Header("Burst shot �������")]
    public bool burstAvalible;//�Ƿ���������ģʽ
    public bool burstActive;//�Ƿ�������ģʽ
    public int burstBulletsPerShot;//����ģʽÿ������ӵ�����
    public float burstFireRate;//����ģʽ����ٶ�
    public float burstFireDelay = .1f;//�����ӳ�

    [Header("Weapon spread ������ɢ")]
    public float baseSpread;//ԭʼ��ɢ��
    public float maxSpread;//�����ɢ��
    public float SpreadIncreaseRate = .15f;//��ɢ������

    [Header("Weapon generics ����ͨ��")]
    public WeaponType weaponType;//��������
    [Range(1f, 3f)]
    public float reloadSpeed = 1;//�����ٶ�
    [Range(1f, 3f)]
    public float equipmentSpeed = 1;//����װ���ٶ�
    [Range(4, 8)]
    public float gunDistance = 4;//�������
    [Range(4, 8)]
    public float cameraDistance = 6;//�������

}
