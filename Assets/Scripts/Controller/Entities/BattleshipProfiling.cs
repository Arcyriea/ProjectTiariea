using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipProfiling : MonoBehaviour, EffectsManager
{
    private SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected EntityAI commandAI;
    protected AudioSource audioSource;

    protected LinkedList<EffectsManager.StatusEffect> statusEffects = new LinkedList<EffectsManager.StatusEffect>();
    public Object propertyForEntity { get; private set; }

    public GameObject barPrefab { get; set; }
    public HealthBar healthBar { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
