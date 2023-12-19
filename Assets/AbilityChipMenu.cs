using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityChipMenu : MonoBehaviour
{
    public Transform playerAnchor;
    public Transform enemyAnchor;


    public GameObject abilityChipPrefab;
    
    IEnumerator MainRoutine(List<Battler> battlers)
    {
        foreach(Battler battler in battlers)
        {
            
        }

        while(BattleManager.Instance.inSkirmish)
        {

        }
    }
}
