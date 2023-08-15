using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GenericActions
{
    public static void MeleeAttack(Transform meleeAttackPoint, Enums.Team team, Object obj)
    {
        if (meleeAttackPoint == null) return;

        Character character = obj.GetType() == typeof(Character) ? (Character) obj : null;
        Enemy enemy = obj.GetType() == typeof(Enemy) ? (Enemy) obj : null;

        float Range, Damage;

        if (character != null && enemy == null)
        {
            Range = character.meleeRange;
            Damage = character.meleeDamage;
        } else if(enemy != null && character == null)
        {
            Range = enemy.attackRange;
            Damage = enemy.attackDamage;
        } else
        {
            UnityEngine.Debug.LogError("Invalid Object Type for MeleeAttack method of static GenericActions class");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, Range, 0);
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        foreach (Collider2D enmity in hitEnemies)
        {
            EnemyProfiling entity = enmity.GetComponent<EnemyProfiling>();
            CharacterProfiling chara = enmity.GetComponent<CharacterProfiling>();

            if (entity != null)
            {
                if (entity.team != team) entity.TakeDamage(Damage);
            }

            if (chara != null)
            {
                if (chara.team != team) chara.TakeDamage(Damage);
            }
        }
    }

    public static void BulletAttack(BulletProperties bullet, Enums.Team team, Object obj, GameObject bulletGO, Vector3 bulletDirection)
    {
        Character character = obj.GetType() == typeof(Character) ? (Character) obj : null;
        Enemy enemy = obj.GetType() == typeof(Enemy) ? (Enemy) obj : null;

        float Range, Damage, Speed, Explosion;

        if (character != null && enemy == null)
        {
            Range = character.shootingRange;
            Damage = character.rangedDamage;
            Speed = character.accelerateSpeed;
            Explosion = character.explodeRadius;
        }
        else if (enemy != null && character == null)
        {
            Range = enemy.attackRange;
            Damage = enemy.attackDamage;
            Speed = enemy.projectileSpeed;
            Explosion = enemy.splashRadius;
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid Object Type for MeleeAttack method of static GenericActions class");
            return;
        }

        if (bullet == null)
        {
            UnityEngine.Debug.Log(character.characterName + " bullet property not defined!");
            return;
        }

        //GameObject bulletGO = Instantiate(bullet.bulletPrefab, transform.position, Quaternion.identity);

        // Get the BulletController component from the instantiated bullet
        BulletController bulletController = bulletGO.GetComponent<BulletController>();

        if (bulletController != null)
        {
            bulletController.Initialize("", team, Damage, Range / Speed, Speed, Explosion, bullet.intercept, bullet.penetrate);
            bulletController.SetDirection(bulletDirection);

            // Rotate the bullet sprite to match the initial direction
            float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            bulletGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public static void BeamAttack()
    {

    }

    public static void HealingAction()
    {

    }
}
