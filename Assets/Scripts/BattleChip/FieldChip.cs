using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class FieldChip
{
    public FieldChipList index;
    public int tile = 0;
    public bool isPlaced;
    public bool isActivated;
    
    public FieldChip(FieldChipList index = FieldChipList.None)
    {
        this.index = index;
        isPlaced = false;
    }
}