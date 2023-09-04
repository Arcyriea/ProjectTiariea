using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalSoundManager : MonoBehaviour
{
    public static AudioSource GlobalSoundPlayer;
    private static Collider2D[] colliders;
    private void Start()
    {
        GlobalSoundPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Detect all colliders within the specified radius.
        colliders = Physics2D.OverlapCircleAll(transform.position, GlobalSoundPlayer.maxDistance);
        
    }

    public static bool IsWithinRange(GameObject gameObject)
    {
        foreach (Collider2D collider in colliders) {
            if (collider != null)
            if (collider.gameObject == gameObject) return true;
        }
        return false;
    }
    // Start is called before the first frame update
}
