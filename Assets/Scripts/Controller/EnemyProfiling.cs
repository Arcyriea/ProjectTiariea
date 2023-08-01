using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class EnemyProfiling : MonoBehaviour
{
    public Enemy enemyData;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float despawnDistance = 75f;

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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyData.EnemyBehavior();
        CheckDespawn();
    }

    private void CheckDespawn()
    {
        // Get the camera's position and add an offset to move it to the left side
        Vector3 cameraPosition = Camera.main.transform.position;
        cameraPosition.z = 0;
        Vector3 despawnPosition = cameraPosition + new Vector3(-despawnDistance, 0f, 0f);

        // Loop through all spawned enemies and check their distance to the left of the camera
        
           
        float distanceToDespawn = transform.position.x - despawnPosition.x;
        UnityEngine.Debug.Log("Despawn Distance = " + distanceToDespawn);
        if (distanceToDespawn < -despawnDistance) Destroy(gameObject);
                    
            
    }
            
}
    
