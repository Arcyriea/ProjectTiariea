using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Canvas canvas;
    public RectTransform rectTransform;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider shieldBar;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 vector3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
