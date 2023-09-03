using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemProfiling : MonoBehaviour
{
    public GameObject parent { get; private set; }
    public Enemy enemyData { get; private set; }
    protected Animator animator;
    protected AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    public Enums.Team team { get; private set; }

    public float Health { get; private set; }
    public float Shield { get; private set; }
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
        animator = enemyData == null ? null : gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformRanged()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformHeal()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformUltimate()
    {
        throw new System.NotImplementedException();
    }
}
