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
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        bossName.SetText("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
