using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemProfiling : MonoBehaviour
{
    public GameObject parent { get; private set; }
    public Enemy enemyData { get; protected set; }
    protected Animator animator;
    protected AudioSource audioSource;
    protected SpriteRenderer spriteRenderer;
    private Transform anchoredTransform;
    public Enums.Team team { get; protected set; }

    public float Health { get; protected set; }
    public float Shield { get; protected set; }
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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(TrackAnchorPoint());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Health <= 0) Destroy(gameObject);
    }

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetAnchorPoint(Transform transform)
    {
        anchoredTransform = transform;
    }

    private IEnumerator TrackAnchorPoint()
    {
        while (true)
        {
            if (anchoredTransform != null) transform.position = anchoredTransform.position;
            yield return null;
        }
    }

    public void TakeDamage(float damage)
    {
        if (Shield > 0) Shield -= damage;
        Health -= Shield <= 0 ? (damage + Shield) : damage;
    }

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
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
