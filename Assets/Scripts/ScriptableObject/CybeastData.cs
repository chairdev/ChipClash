using UnityEngine;

[CreateAssetMenu(fileName = "Cybeast", menuName = "ScriptableObjects/Cybeast", order = 1)]
public class CybeastData : ScriptableObject
{
    public string beastName;
    public int basePL;


}

public enum CybeastList
{
    None,
    Pantheron,
}

public enum Elements
{
    None,
    Ignis,
    Aqua,
    Eurus,
    Electrum,
    Terra,
    Lux,
    Nox,
}