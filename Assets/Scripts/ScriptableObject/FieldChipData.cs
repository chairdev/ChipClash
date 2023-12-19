using UnityEngine;

[CreateAssetMenu(fileName = "Field Chip", menuName = "ScriptableObjects/FieldChip", order = 1)]
public class FieldChipData : ScriptableObject
{
    public string chipName;
    [TextArea(3, 10)] public string description;

    public FieldChipEffect[] chipEffects;
}

[System.Serializable]
public struct FieldChipEffect
{
    public FldChipEffects effect;
    public int[] value;
}

public enum FldChipEffects
{
    None,
    RaisePowerOfUser,
}

public enum FieldChipList
{
    None,
    Test,
    PowerPlus,
    IgnisCore,
    AquaCore,
    EurusCore,
    ElectrumCore,
    TerraCore,
    LuxCore,
    NoxCore,
}