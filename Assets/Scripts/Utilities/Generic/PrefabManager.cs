using UnityEngine;

public static class PrefabManager
{
    public static GameObject warpOut;
    public static GameObject beamCharge;

    // Assign prefab references through code or in the Inspector
    public static void Initialize()
    {
        warpOut = Resources.Load<GameObject>("Effects/Warpout");
        beamCharge = Resources.Load<GameObject>("Effects/Beam Chargeup");
        UnityEngine.Debug.Log("Resource Asset Initialized");
    }
}