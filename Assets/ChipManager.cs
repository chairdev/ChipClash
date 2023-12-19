using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipManager : MonoBehaviour
{
    public FieldChipData[] fieldChips;
    public AbilityChipData[] abilityChips;

    public static ChipManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum ChipType
{
    Field,
    Ability
}