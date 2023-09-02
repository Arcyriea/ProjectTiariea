using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    public static AudioSource GlobalSoundPlayer;
    private void Start()
    {
        GlobalSoundPlayer = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
}
