using UnityEngine;

[CreateAssetMenu(fileName = "SavedSeed", menuName = "UnitySDK/SavedSeed", order = 0)]
public class SavedSeed : ScriptableObject
{
    public Random.State state;
}