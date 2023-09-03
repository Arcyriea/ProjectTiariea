using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemProfiling : MonoBehaviour
{
    public GameObject parent { get; private set; }
    public Enemy enemyData { get; private set; }
    protected Animator animator;
    protected AudioSource audioSource;
    protected SpriteRenderer spriteRenderer;
    private Transform anchoredTransform;
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
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(TrackAnchorPoint());
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
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
