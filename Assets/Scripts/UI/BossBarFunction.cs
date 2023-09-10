using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBarFunction : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;

    public TextMeshProUGUI bossName;
    public Object bossProfiling { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        bossName.SetText("");
    }

    // Update is called once per frame
    void Start()
    {
        if (bossProfiling is EnemyProfiling bossEntity)
        {
            bossName.SetText(bossEntity.enemyData.enemyName);
            InitializeSliders(bossEntity.enemyData.maximumHealth, bossEntity.enemyData.maximumShield);
        }
        else if (bossProfiling is CharacterProfiling bossCharacter)
        {
            bossName.SetText(bossCharacter.character.characterName);
            InitializeSliders(bossCharacter.character.maximumHealth, bossCharacter.character.maximumShield);
        }
        else if (bossProfiling is BattleshipProfiling bossBattleship)
        {
            bossName.SetText(bossBattleship.battleshipProperty.batteshipName);
            InitializeSliders(bossBattleship.battleshipProperty.maximumHealth, bossBattleship.battleshipProperty.maximumShield);
        }
    }

    void Update()
    {
        if (bossProfiling != null)
        {
            UpdateSliders(bossProfiling);
        }
    }

    private void UpdateSliders(Object bossProfiling)
    {
        if (bossProfiling is EnemyProfiling entityProfiling)
        {
            if (healthSlider.value != entityProfiling.Health) healthSlider.value = entityProfiling.Health;
            if (shieldSlider.value != entityProfiling.Shield) shieldSlider.value = entityProfiling.Shield;
        }
        else if (bossProfiling is CharacterProfiling characterProfiling)
        {
            if (healthSlider.value != characterProfiling.Health) healthSlider.value = characterProfiling.Health;
            if (shieldSlider.value != characterProfiling.Shield) shieldSlider.value = characterProfiling.Shield;
        } 
        else if (bossProfiling is BattleshipProfiling battleshipProfiling)
        {
            if (healthSlider.value != battleshipProfiling.Health) healthSlider.value = battleshipProfiling.Health;
            if (shieldSlider.value != battleshipProfiling.Shield) shieldSlider.value = battleshipProfiling.Shield;
        }
        
    }

    void InitializeSliders(float maxHealth, float maxShield)
    {
        healthSlider.maxValue = maxHealth;
        shieldSlider.maxValue = maxShield;
    }

    public void SetBossProfile(Object bossProfiling)
    {
        this.bossProfiling = bossProfiling;

        if (!gameObject.activeSelf)
        {

            if (bossProfiling is EnemyProfiling bossEntity)
            {
                bossName.SetText(bossEntity.enemyData.enemyName);
                InitializeSliders(bossEntity.enemyData.maximumHealth, bossEntity.enemyData.maximumShield);
            }
            else if (bossProfiling is CharacterProfiling bossCharacter)
            {
                bossName.SetText(bossCharacter.character.characterName);
                InitializeSliders(bossCharacter.character.maximumHealth, bossCharacter.character.maximumShield);
            } 
            else if (bossProfiling is BattleshipProfiling bossBattleship)
            {
                bossName.SetText(bossBattleship.battleshipProperty.batteshipName);
                InitializeSliders(bossBattleship.battleshipProperty.maximumHealth, bossBattleship.battleshipProperty.maximumShield);   
            }
        }
    }

    public void RemoveBoss()
    {
        bossProfiling = null;
        bossName.SetText("");
        InitializeSliders(0, 0);
        gameObject.SetActive(false);
    }

}
