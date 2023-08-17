using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarFunctions : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;
    public Slider manaSlider;
    public Slider energySlider;

    public CharacterProfiling characterProfiling { get; private set; }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    void Initialize(float maxHealth, float maxShield, float maxMana, float maxEnergy)
    {
        gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        shieldSlider.maxValue = maxShield;
        manaSlider.maxValue = maxMana;
        energySlider.maxValue = maxEnergy;
    }

    public void SetCharProfile(CharacterProfiling characterProfiling)
    {
        this.characterProfiling = characterProfiling;
    }

    public void Update()
    {
        if (characterProfiling != null && gameObject.activeSelf == false)
        {
            
            Initialize(characterProfiling.character.maximumHealth, 
                characterProfiling.character.maximumShield,
                characterProfiling.character.maximumMana,
                characterProfiling.character.maximumEnergy);
        }
    }

    public void RemoveChar()
    {
        characterProfiling = null;
        healthSlider.maxValue = 0;
        shieldSlider.maxValue = 0;
        manaSlider.maxValue = 0;
        energySlider.maxValue = 0;
        gameObject.SetActive(false);
    }
}
