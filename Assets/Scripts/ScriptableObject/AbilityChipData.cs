using UnityEngine;

[CreateAssetMenu(fileName = "AbilityChipData", menuName = "ScriptableObjects/AbilityChipData")]
public class AbilityChipData : ScriptableObject
{
    public string chipName;
    [TextArea(3, 10)] public string description;
}

public enum AbilityChipList
{
    None,
    Test,
    ElementWheel,
    BurningHeart,
}
