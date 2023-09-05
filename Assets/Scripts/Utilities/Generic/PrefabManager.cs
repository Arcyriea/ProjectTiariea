using UnityEngine;

public static class PrefabManager
{
    public static GameObject warpOut;
    //public static GameObject prefab2;

    // Assign prefab references through code or in the Inspector
    public static void Initialize()
    {
        warpOut = Resources.Load<GameObject>("Effects/Warpout");
        //prefab2 = Resources.Load<GameObject>("Prefab2");
        UnityEngine.Debug.Log("Resource Asset Initialized");
    }
}