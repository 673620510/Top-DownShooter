using System;
using UnityEngine;

//****************************************
//创建人：逸龙
//功能说明：
//****************************************
public class Dummy : MonoBehaviour, IDamagable
{
    public int currentHealth;
    public int maxHealth = 100;

    [Header("Material")]
    public MeshRenderer mesh;
    public Material whiteMat;
    public Material redMat;
    [Space]
    public float refreshCooldown;
    private float lastTimeDamage;
    private void Start()
    {
        Refresh();
    }
    private void Update()
    {
        if (Time.time > refreshCooldown + lastTimeDamage || Input.GetKeyDown(KeyCode.B))
        {
            Refresh();
        }
    }
    private void Refresh()
    {
        currentHealth = maxHealth;
        mesh.material = whiteMat;
    }

    public void TakeDamage(int damage)
    {
        lastTimeDamage = Time.time;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        mesh.material = redMat;
    }
}
