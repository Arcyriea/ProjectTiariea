using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KiajuyianaActions : CharacterProfiling, IDefaultActions
{
    public BulletProperties bullet;
    public Transform meleeAttackPoint;
    

    public override void CharacterAction(string action)
    {
        switch (action)
        {
            case "attack":
                PerformAttack();
                break;
            case "heal":
                PerformHeal();
                break;
            case "ranged":
                PerformRanged();
                break;
            case "ultimate":
                PerformUltimate();
                break;
            default:
                UnityEngine.Debug.LogError("Invalid action: " + action);
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (base.moveToMouse.selected == true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
            {
                CharacterAction("ranged");
            }

            if (Input.GetMouseButtonDown(1))
            {
                CharacterAction("attack");
            }
        }
    }

    public void PerformAttack()
    {
        if (meleeAttackPoint == null) return;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, base.character.meleeRange, 0);
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyProfiling entity = enemy.GetComponent<EnemyProfiling>();
            CharacterProfiling chara = enemy.GetComponent<CharacterProfiling>();

            if (entity != null)
            {
                if (entity.team != team) entity.TakeDamage(base.character.meleeDamage);
            }

            if (chara != null)
            {
                if (chara.team != team) chara.TakeDamage(base.character.meleeDamage);
            }
        }

        UnityEngine.Debug.Log(character.characterName + " performs an attack!");
    }

    public void PerformRanged()
    {
        if (bullet == null)
        {
            UnityEngine.Debug.Log(character.characterName + " bullet property not defined!");
            return;
        }

        GameObject bulletGO = Instantiate(bullet.bulletPrefab, transform.position, Quaternion.identity);

        // Get the BulletController component from the instantiated bullet
        BulletController bulletController = bulletGO.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.Initialize("", base.team, base.character.rangedDamage, base.character.shootingRange / 0.5f, 0.5f, 0f, bullet.intercept, bullet.penetrate);

            Vector3 bulletDirection = Vector3.right;
            bulletController.SetDirection(bulletDirection);

            // Rotate the bullet sprite to match the initial direction
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            bulletGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        UnityEngine.Debug.Log(character.characterName + " performs ranged attack!");
    }

    public void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log(character.characterName + " performs a heal!");
    }

    public void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log(character.characterName + " performs their ultimate ability!");
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null) return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, 1.5f);
    }

}
