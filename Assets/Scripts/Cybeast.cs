using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Cybeast
{
    public int owner;
    public CybeastList index;
    public Elements attribute;
    public int powerLevel;

    public int tile;

    public BeastStatus status = BeastStatus.InPocket; 
    
    public Cybeast(CybeastList index = CybeastList.None, Elements attribute = Elements.None, int powerLevel = 0)
    {
        this.index = index;
        this.attribute = attribute;
        this.powerLevel = powerLevel;
    }    

    public string GetName()
    {
        return CybeastManager.Instance.cybeasts[(int)index].beastName;
    }

    public void Set(int tile)
    {
        this.tile = tile;
        status = BeastStatus.OnField;
    }
}  

public enum BeastStatus
{
    None,
    InPocket,
    OnField,
    InBattle,
    KnockedOut,
}