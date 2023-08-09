using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HomeworldHearts : MonoBehaviour
{
    public int heart;
    public int numOfHearts;

    public TextMeshProUGUI textHeart;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private enum HeartCount { O, I, II, III, IV, V, VI, VII, VIII, IX, X }
    private static HeartCount[] enumValues = (HeartCount[])Enum.GetValues(typeof(HeartCount));

    private static HeartCount heartCount = enumValues[0];
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (heart > numOfHearts) heart = numOfHearts;

        UpdateHearts();
        UpdateTextHeart();
    }

    private void UpdateHearts()
    {
        if (heart == 0)
            {
                foreach(Image hrts in hearts){
                hrts.sprite = emptyHeart;
                }
            }
        else
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i == 0 && heart >= 10)
                {
                    hearts[i].sprite = fullHeart;
                }
                else if (i <= heart % 10 || (i == 0 && heart < 10))
                {
                    hearts[i].sprite = fullHeart;
                }
                else if (heart >= 100)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }
        
    }

    private void UpdateTextHeart()
    {
        int tier = heart > 100 ? 10 : (heart) / 10; // Calculate the tier based on heart count

        
        if (tier < enumValues.Length)
        {
            heartCount = enumValues[tier];

            if (heart > 100) textHeart.text = heartCount.ToString() + ": " +  heart.ToString();
            else textHeart.text = heartCount.ToString();
        }
    }
}
