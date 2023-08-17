using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarFunctions : MonoBehaviour
{
    public Slider healthSlider;
    public Slider shieldSlider;
    public Slider manaSlider;
    public Slider energySlider;

    public TextMeshProUGUI charName;

    public CharacterProfiling characterProfiling { get; private set; }

    private void Awake()
    {
        gameObject.SetActive(false);
        charName.SetText("");
    }

    private void Start()
    {
        if (characterProfiling != null)
        {
            charName.SetText(characterProfiling.character.characterName);
            InitializeSliders(
                characterProfiling.character.maximumHealth,
                characterProfiling.character.maximumShield,
                characterProfiling.character.maximumMana,
                characterProfiling.character.maximumEnergy
            );
        }
    }

    private void Update()
    {
        if (characterProfiling != null)
        {
            UpdateSliders(characterProfiling);
        }
    }

    private void UpdateSliders(CharacterProfiling characterProfiling)
    {
        if (healthSlider.value != characterProfiling.Health) healthSlider.value = characterProfiling.Health;
        if (shieldSlider.value != characterProfiling.Shield) shieldSlider.value = characterProfiling.Shield;
        if (manaSlider.value != characterProfiling.Mana) manaSlider.value = characterProfiling.Mana;
        if (energySlider.value != characterProfiling.Energy) energySlider.value = characterProfiling.Energy;
    }

    public void SetCharProfile(CharacterProfiling characterProfiling)
    {
        this.characterProfiling = characterProfiling;

        if (!gameObject.activeSelf)
        {

            InitializeSliders(
                characterProfiling.character.maximumHealth,
                characterProfiling.character.maximumShield,
                characterProfiling.character.maximumMana,
                characterProfiling.character.maximumEnergy
            );
            charName.SetText(characterProfiling.character.characterName);
        }
    }

    public void RemoveChar()
    {
        characterProfiling = null;
        charName.SetText("");
        InitializeSliders(0, 0, 0, 0);
        gameObject.SetActive(false);
    }

    private void InitializeSliders(float maxHealth, float maxShield, float maxMana, float maxEnergy)
    {
        gameObject.SetActive(true);
        healthSlider.maxValue = maxHealth;
        shieldSlider.maxValue = maxShield;
        manaSlider.maxValue = maxMana;
        energySlider.maxValue = maxEnergy;
    }
}
