using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;

public class EnemyProfiling : MonoBehaviour
{
    public Enemy enemyData { get; private set; }
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float despawnDistance = 75f;
    private HomeworldHearts homeworld;

    private class StatusEffect
    {
        public Enums.StatusEffectType type;
        public int stackCount;
        public float duration;

        public StatusEffect(Enums.StatusEffectType type, int stackCount, float duration)
        {
            this.type = type;
            this.stackCount = stackCount;
            this.duration = duration;
        }
    }

    private LinkedList<StatusEffect> statusEffects = new LinkedList<StatusEffect>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        homeworld = GameObject.Find("Main Camera")?.GetComponent<HomeworldHearts>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDespawn();
    }

    public void SetEnemyData(Enemy enemy)
    {
        enemyData = enemy;
    }

    private void CheckDespawn()
    {
        

        // Loop through all spawned enemies and check their distance to the left of the camera
        
           
        float distanceToDespawn = transform.position.x - Camera.main.transform.position.x;
        if (distanceToDespawn < -despawnDistance)
        {
            Destroy(gameObject);
            if (homeworld != null)
            {
                homeworld.heart -= 1;
            }
            else
            {
                UnityEngine.Debug.LogWarning("HomeworldHearts component not found on Main Camera.");
            }
        }
    }

    public virtual void EnemyAction(string action)
    {
        UnityEngine.Debug.Log("Got into EnemyAction");
    }
            
}
    
