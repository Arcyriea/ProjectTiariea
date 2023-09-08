using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsystemProfiling : MonoBehaviour, EffectsManager
{
    public GameObject parent { get; private set; }
    public Subsystem subsystemData;
    protected Animator animator;
    protected AudioSource audioSource;
    protected SpriteRenderer spriteRenderer;
    private Transform anchoredTransform;
    public Enums.Team team { get; protected set; }

    public float Health { get; protected set; }
    public float Shield { get; protected set; }

    protected LinkedList<EffectsManager.StatusEffect> statusEffects = new LinkedList<EffectsManager.StatusEffect>();
    // Start is called before the first frame update
    public GameObject barPrefab { get; set; }
    public HealthBar healthBar { get; protected set; }
    protected virtual void Start()
    {
        animator = subsystemData == null ? null : gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (anchoredTransform != null) StartCoroutine(TrackAnchorPoint());
        Health = subsystemData.maximumHealth;
        Shield = subsystemData.maximumShield;
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
        else Health -= Shield <= 0 ? (damage + Shield) : damage;
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
