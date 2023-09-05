using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Beam")]
public class BeamProperties : ScriptableObject
{
    // Start is called before the first frame update
    public GameObject beamPrefab;
    public bool intercept;
    public bool penetrate;
}
