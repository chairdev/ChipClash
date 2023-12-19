using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Battler
{
    public string name;
    public AIType aiType;
    public int lifePoints;
    public int gravityPoints;
    public Team teamID;
    
    public List<Cybeast> cybeasts = new List<Cybeast>();
    
    public List<FieldChip> fieldChips = new List<FieldChip>();
    public List<AbilityChip> abilityChips = new List<AbilityChip>();

    public Battler(string name, Team teamID, int lifePoints = 500, int gravityPoints = 0, List<Cybeast> cybeasts = null, List<FieldChip> fieldChips = null, List<AbilityChipList> abilityChips = null)
    {
        this.name = name;
        this.lifePoints = lifePoints;
        this.gravityPoints = gravityPoints;
        this.teamID = teamID;

        if(cybeasts != null)
        {
            this.cybeasts = cybeasts;
        }

        if(fieldChips != null)
        {
            this.fieldChips = fieldChips;
        }

        if(abilityChips != null)
        {
            for(int i = 0; i < abilityChips.Count; i++)
            {
                AbilityChip chip = new AbilityChip(abilityChips[i]);
                this.abilityChips.Add(chip);
            }
        }
    }

    public FieldChipList GetFieldChip(int index)
    {
        return fieldChips[index].index;
    }

    public AbilityChipList GetAbilityChip(int index)
    {
        return abilityChips[index].index;
    }
    
    public int GetFieldChipPlacedOnTile(int tile)
    {
        for (int i = 0; i < fieldChips.Count; i++)
        {
            if(fieldChips[i].tile == tile)
            {
                return i;
            }
        }

        return -1;
    }

    public List<int> GetValidChips(ChipType chipType)
    {
        List<int> validChips = new List<int>();

        switch (chipType)
        {
            case ChipType.Field:
            for (int i = 0; i < fieldChips.Count; i++)
                {
                    if(!fieldChips[i].isPlaced)
                    {
                        validChips.Add(i);
                    }
                }
            break;
            case ChipType.Ability:
                for (int i = 0; i < abilityChips.Count; i++)
                {
                    validChips.Add(i);
                }
            break;

        }

        return validChips;
    }

    public bool CanPlaceAnyChips(ChipType type)
    {
       return GetValidChips(type).Any();
    }

    public void PlaceChip(ChipType chipType, int index)
    {
        if(chipType == ChipType.Field)
        {
            fieldChips[index].isPlaced = true;
        }
    }

    public void RemoveChip(ChipType chipType, int index)
    {
        if(chipType == ChipType.Field)
        {
            fieldChips[index].isPlaced = false;
            fieldChips[index].isActivated = false;
        }
    }

    public int GetChipCount(ChipType chipType)
    {
        if(chipType == ChipType.Field)
        {
            return fieldChips.Count;
        }
        else
        {
            return abilityChips.Count;
        }
    }


}

public enum Team
{
    Spectator,
    Player,
    Enemy
}

public enum AIType
{
    Player,
    Random,
    Aggressive,
    Defensive
}