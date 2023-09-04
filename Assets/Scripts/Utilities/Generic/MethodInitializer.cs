using System;
using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;

public static class GenericActions
{
    public static void ExecuteAction(CharacterProfiling profiling, string action)
    {
        Character character = profiling.character;
        float meleeAttackTime = profiling.meleeAttackTime;
        float rangedAttackTime = profiling.rangedAttackTime;
        switch (action)
        {
            case "attack":
                if (Time.time - meleeAttackTime >= character.swingTime)
                {
                    profiling.PerformAttack();
                    profiling.meleeAttackTime = Time.time;
                }
                break;
            case "heal":
                if (Time.time - rangedAttackTime >= character.fireRate)
                {
                    profiling.PerformHeal();
                    profiling.rangedAttackTime = Time.time;
                }
                break;
            case "ranged":
                if (Time.time - rangedAttackTime >= character.fireRate)
                {
                    profiling.PerformRanged();
                    profiling.rangedAttackTime = Time.time;
                }
                break;
            case "ultimate":
                if (profiling.UltimateTimer >= profiling.UltimateMeter)
                {
                    profiling.PerformUltimate();
                }
                break;
            default:
                UnityEngine.Debug.LogError("Invalid action: " + action);
                break;
        }
    }

    public static void MeleeAttack(Transform meleeAttackPoint, Team team, UnityEngine.Object obj)
    {
        if (meleeAttackPoint == null) return;

        Character character = obj.GetType() == typeof(Character) ? (Character)obj : null;
        Enemy enemy = obj.GetType() == typeof(Enemy) ? (Enemy)obj : null;

        float Range, Damage;

        if (character != null && enemy == null)
        {
            Range = character.meleeRange;
            Damage = character.meleeDamage;
        }
        else if (enemy != null && character == null)
        {
            Range = enemy.attackRange;
            Damage = enemy.attackDamage;
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid Object Type for MeleeAttack method of static GenericActions class");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleeAttackPoint.position, Range);

        foreach (Collider2D enmity in hitEnemies)
        {
            EnemyProfiling entity = enmity.GetComponent<EnemyProfiling>();
            CharacterProfiling chara = enmity.GetComponent<CharacterProfiling>();

            if (entity != null)
            {
                if (entity.team != team)
                {
                    entity.TakeDamage(Damage);
                }
            }

            if (chara != null)
            {
                if (chara.team != team)
                {
                    chara.TakeDamage(Damage);
                }
            }
        }
    }

    public static void BulletAttack(BulletProperties bullet, Team team, UnityEngine.Object obj, GameObject bulletGO, Vector3 bulletDirection)
    {
        Character character = obj.GetType() == typeof(Character) ? (Character)obj : null;
        Enemy enemy = obj.GetType() == typeof(Enemy) ? (Enemy)obj : null;

        float Range, Damage, Speed, Explosion;

        if (character != null && enemy == null)
        {
            Range = character.shootingRange;
            Damage = bullet.damage == 0 ? character.rangedDamage : bullet.damage;
            Speed = character.accelerateSpeed + PartyController.partyTravelSpeed;
            Explosion = character.explodeRadius;
        }
        else if (enemy != null && character == null)
        {
            Range = enemy.attackRange;
            Damage = bullet.damage == 0 ? enemy.attackDamage : bullet.damage;
            Speed = enemy.projectileSpeed;
            Explosion = enemy.splashRadius;
        }
        else
        {
            UnityEngine.Debug.LogError("Invalid Object Type for BulletAttack method of static GenericActions class");
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
            float angle = Mathf.Atan2(bulletController.direction.y, bulletController.direction.x) * Mathf.Rad2Deg;
            bulletGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    

    public static void MissileAttack(MissileProperties missileProperties, Team team, UnityEngine.Object obj, GameObject missileGO, Vector3 missileDirection, GameObject parentTransform)
    {
        MissileController missileController = missileGO.GetComponent<MissileController>();
        if (missileController != null)
        {
            missileController.Initialize("", obj, team, missileProperties, parentTransform);
            missileController.SetDirection(missileDirection);

            float angle = Mathf.Atan2(missileController.direction.y, missileController.direction.x) * Mathf.Rad2Deg;
            missileGO.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public static void BeamAttack(float duration, LaserController controller)
    {
        if (controller == null) { return; }

        if (duration > 0)
        {
            controller.duration = duration;
            if (!controller.lineRenderer.enabled)
            {
                controller.lineRenderer.enabled = true;
            }

            duration -= Time.deltaTime;

            if (duration <= 0)
            {
                controller.lineRenderer.enabled = false;
            }
        }
    }

    public static void HealingAction()
    {
        
    }

    public static IEnumerator ChargingUp(IEnumerator coroutine, float chargeDuration, AudioClip chargeAudioClip, AudioClip firingAudioClip)
    {
        if (chargeAudioClip != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(chargeAudioClip);
        yield return new WaitForSeconds(chargeDuration);
        if (firingAudioClip != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(firingAudioClip);
        yield return coroutine;
        yield break;
    }
}

public static class Ultilty
{
    public static bool CheckIfVariableExists(object obj, string variableName)
    {
        Type type = obj.GetType();
        FieldInfo field = type.GetField(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return field != null;
    }
}
