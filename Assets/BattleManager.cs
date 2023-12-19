using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    
    public GameObject[] phasePopups;
    public GameObject tilePrefab;
    public RectTransform[] infoAnchors;

    public GameObject cyCardPrefab;
    public List<Cycard> playerCycards;
    public List<Cycard> enemyCycards;

    public GameObject battleField;

    public BattleChipSelect chipSelect;
    public TurnAnnouncer turnAnnouncer;
    public TurnSelect turnSelect;

    public bool inSkirmish = false;

    public int turn = 0;
    public int battleInitiator = 0;
    List<int> turnOrder = new List<int>();

    public BattlerInfo[] battlerInfo = new BattlerInfo[2];
    public FieldTile[] fieldTiles;
    
    public List<Battler> battlers = new List<Battler>();

    public static BattleManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

        // Start is called before the first frame update
    IEnumerator Start()
    {
        AudioManager.Instance.PlayBGM(BGM.NORMALARENA);
        yield return StartCoroutine(BattleRoutine(battlers));
    }

    public IEnumerator SpawnCycard(Cybeast cybeast, bool isPlayer)
    {
        string parent;
        bool skip = false;

        if(isPlayer)
        {
            parent = "PlayerCycards";

            for (int i = 0; i < playerCycards.Count; i++)
            {
                if(playerCycards[i].beast == cybeast)
                {
                    skip = true;
                }
            }
        }
        else
        {
            parent = "EnemyCycards";

            for (int i = 0; i < enemyCycards.Count; i++)
            {
                if(enemyCycards[i].beast == cybeast)
                {
                    skip = true;
                }
            }
        }

        if(skip)
        {
            yield break;
        }

        GameObject card = Instantiate(cyCardPrefab, GameObject.Find(parent).transform);
        RectTransform cardTransform = card.GetComponent<RectTransform>();
        cardTransform.DORotate(new Vector3(0f, 0f, 0f), 0.2f);

        card.GetComponent<Cycard>().beast = cybeast;

        if(isPlayer)
        {
            playerCycards.Add(card.GetComponent<Cycard>());
        }
        else
        {
            enemyCycards.Add(card.GetComponent<Cycard>());
        }

        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator DestroyCycards()
    {
        for (int i = 0; i < playerCycards.Count; i++)
        {
            yield return StartCoroutine(DestroyCard(playerCycards[i].gameObject));
        }

        for (int i = 0; i < enemyCycards.Count; i++)
        {
            yield return StartCoroutine(DestroyCard(enemyCycards[i].gameObject));
        }

        playerCycards = new List<Cycard>();
        enemyCycards = new List<Cycard>();
    }

    IEnumerator DestroyCard(GameObject card)
    {
        RectTransform cardTransform = card.GetComponent<RectTransform>();
        cardTransform.DORotate(new Vector3(0f, 90f, 0f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        Destroy(card);
    }

    IEnumerator SpawnField()
    {
        GameObject[] row = new GameObject[3];
        row[0] = GameObject.Find("Row 1");
        row[1] = GameObject.Find("Row 2");
        row[2] = GameObject.Find("Row 3");

        for (int i = 0; i < 3*3; i++)
        {
            fieldTiles[i] = Instantiate(tilePrefab, row[i%3].transform).GetComponent<FieldTile>();
            CameraManager.Instance.Target = fieldTiles[i].transform;
            yield return new WaitForSeconds(0.1f);
        }
        //go back to middle
        CameraManager.Instance.Target = fieldTiles[4].transform;
        yield return null;
    }

    public IEnumerator SlideBattlerInfo(Team team, bool slideIn)
    {
        if (slideIn)
        {
            infoAnchors[(int)team].DOAnchorPosX(0f, 0.2f);
        }
        else
        {
            if(team == Team.Player)
            {
                infoAnchors[(int)team].DOAnchorPosX(-100f, 0.2f);
            }
            else
            {
                infoAnchors[(int)team].DOAnchorPosX(100f, 0.2f);
            }
        }

        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator BattleRoutine(List<Battler> battlers, int battleInitiator = 0)
    {
        bool battleOver = false;
        int turn = 0;
        this.battleInitiator = battleInitiator;

        yield return StartCoroutine(SetupBattle());

        while (!battleOver)
        {
            turnOrder = CalculateTurnOrder();

            battleOver = WinCondition();
            if(battleOver)
            {
                Debug.Log("Battle over 0");
                break;
            }

            if(fieldIsEmpty())
            {
                yield return StartCoroutine(ForceSetFieldChip());
            }

            for (int i = 0; i < turnOrder.Count; i++)
            {
                if(WinCondition())
                {
                    Debug.Log("Battle over 1");
                    break;
                }
                
                yield return StartCoroutine(BattleTurn(turnOrder[i]));

                if(WinCondition())
                {
                    Debug.Log("Battle over 2");
                    break;
                }
            }

            battleOver = WinCondition();

            turn++;
            yield return null;
        }
        
        int winner = battlers.FindIndex(x => x.lifePoints > 0);
        yield return StartCoroutine(turnAnnouncer.DoText(battlers[winner].name + " wins!"));




        
        
        


        yield return null;
    }

    bool WinCondition()
    {
        int battlersAlive = 0;

        for (int i = 0; i < battlers.Count; i++)
        {
            if(battlers[i].lifePoints > 0)
            {
                battlersAlive++;
            }
        }

        Debug.Log("Battlers alive: " + battlersAlive);

        if(battlersAlive == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator DisplayCybeastsOnTile(int tile)
    {
        List<Cybeast> cybeasts = GetCybeastsOnTile(tile).OrderBy(x => x.owner == turnSelect.currentBattler ? 0 : 1).ToList();

        for (int i = 0; i < cybeasts.Count; i++)
        {
            yield return StartCoroutine(SpawnCycard(cybeasts[i], cybeasts[i].owner == turnSelect.currentBattler));
        }
    }

    public List<Cybeast> GetCybeastsOnTile(int tile)
    {
        List<Cybeast> cybeasts = new List<Cybeast>();

        for (int i = 0; i < battlers.Count; i++)
        {
            for (int j = 0; j < battlers[i].cybeasts.Count; j++)
            {
                if(battlers[i].cybeasts[j].tile == tile && battlers[i].cybeasts[j].status == BeastStatus.OnField)
                {
                    cybeasts.Add(battlers[i].cybeasts[j]);
                }
            }
        }

        return cybeasts;
    }

    public IEnumerator CheckBattle(int tile)
    {
        List<Cybeast> cybeasts = GetCybeastsOnTile(tile);
        Debug.Log("Cybeasts on tile " + tile + ": " + cybeasts.Count);

        //check if there are 2 cybeasts on the same tile
        if(cybeasts.Count == 2)
        {
            //the cybeast that belongs to the current battler is playerbattler
            Cybeast playerCybeast = cybeasts.Find(x => x.owner == turnSelect.currentBattler);
            Cybeast enemyCybeast = cybeasts.Find(x => x.owner != turnSelect.currentBattler);

            BattleAnnouncer.Instance.AnnounceSkirmishStart();
            yield return StartCoroutine(DisplayPhasePopup(BattlePhase.SKIRMISHSTART));
            yield return StartCoroutine(Skirmish(cybeasts, tile)); 
        }
        else
        {
    
        }

        yield return StartCoroutine(DestroyCycards());
    }

    void ReturnFieldChip(int tile)
    {
        int chipOwner = fieldTiles[tile].chipOwner;
        int index = battlers[chipOwner].GetFieldChipPlacedOnTile(tile);

        battlers[chipOwner].RemoveChip(ChipType.Field, index);
        fieldTiles[tile].DestroyChip();
    }

    public IEnumerator Skirmish(List<Cybeast> cybeasts, int tile)
    {
        AudioManager.Instance.PlayBGM(BGM.BATTLECONFRONTATION);
        //a battle lasts 20 seconds
        float timer = 5f;
        inSkirmish = true;

        battleField.transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        while (inSkirmish)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                inSkirmish = false;
            }

            StartCoroutine(ProcessFieldChip(tile, cybeasts));

            StartCoroutine(chipSelect.MenuRoutine(turnSelect.currentBattler, ChipType.Ability, false, true));

            yield return null;
        }

        chipSelect.returnType = -1;
        chipSelect.inMenu = false; 

        //winner is the cybeast with the most POWER LEVEL
        Cybeast winner = cybeasts.OrderByDescending(x => x.powerLevel).First();

        //loser is the cybeast with the least POWER LEVEL
        Cybeast loser = cybeasts.OrderBy(x => x.powerLevel).First();
        
        //get the difference between the winner and loser's power level
        int difference = winner.powerLevel - loser.powerLevel;

        AudioManager.Instance.PlayBGM(BGM.NORMALARENA);

        //put the cybeasts back in their pockets
        for (int i = 0; i < cybeasts.Count; i++)
        {
            cybeasts[i].status = BeastStatus.InPocket;
        }

        //get the index of the current field chip
        ReturnFieldChip(tile);

        battleField.transform.DOScale(new Vector3(0f, 0f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        battlers[winner.owner].gravityPoints += difference;
        yield return StartCoroutine(DecreaseLifePoints(loser.owner, difference));
    }

    IEnumerator ProcessFieldChip(int tile, List<Cybeast> cybeasts)
    {
        int chipOwner = fieldTiles[tile].chipOwner;
        int index = battlers[chipOwner].GetFieldChipPlacedOnTile(tile);

        FieldChipData data = ChipManager.Instance.fieldChips[(int)battlers[chipOwner].fieldChips[index].index];

        Cybeast beastToEffect;

        if(battlers[chipOwner].fieldChips[index].isActivated)
        {
            yield break;
        }

        foreach (FieldChipEffect effect in data.chipEffects)
        {
            switch (effect.effect)
            {
                case FldChipEffects.RaisePowerOfUser:
                    battlers[chipOwner].fieldChips[index].isActivated = true;

                    int raisePower = effect.value[UnityEngine.Random.Range(0, effect.value.Length)];
                    beastToEffect =  cybeasts.Find(x => x.owner == chipOwner);
                    yield return StartCoroutine(ChangeCybeastPower(beastToEffect, beastToEffect.powerLevel+raisePower));
                break;

                    
            }
        }

        yield return null;
    }

    IEnumerator ChangeCybeastPower(Cybeast cybeast, int amount)
    {
        DOTween.To(() => cybeast.powerLevel, x => cybeast.powerLevel = x, amount, 1f);
        yield return null;
    }

    IEnumerator DecreaseLifePoints(int battlerIndex, int amount)
    {
        //use dotween to decrease the life points float
        int newLifePoints = Mathf.Clamp(battlers[battlerIndex].lifePoints - amount, 0, 500);
        DOTween.To(() => battlers[battlerIndex].lifePoints, x => battlers[battlerIndex].lifePoints = x, newLifePoints, 1f);

        yield return new WaitForSeconds(1f);
    }



    IEnumerator BattleTurn(int battlerIndex)
    {
        int playerIndex = GetBattlerPlayerNumber(battlerIndex);
        int cpuIndex = GetBattlerCPUNumber(battlerIndex);        

        chipSelect.returnType = -1;

        if(playerIndex != -1)
        {
            BattleAnnouncer.Instance.AnnouncePlayerTurn(playerIndex);
        }
        else if(cpuIndex != -1)
        {
            BattleAnnouncer.Instance.AnnounceCPUTurn(cpuIndex);
        }

        yield return StartCoroutine(turnAnnouncer.AnnounceTurn(battlerIndex));
        yield return StartCoroutine(turnSelect.Routine(battlerIndex));

        yield return StartCoroutine(CheckBattle(chipSelect.currentTile));
        
        yield return null;
    }

    bool fieldIsEmpty()
    {
        for (int i = 0; i < fieldTiles.Length; i++)
        {
            if(fieldTiles[i].chipOwner != -1)
            {
                return false;
            }
        }

        return true;
    }

    int GetBattlerPlayerNumber(int battlerIndex)
    {
       List<int> playerNumbers = new List<int>();

        for (int i = 0; i < battlers.Count; i++)
        {
            if(battlers[i].aiType == AIType.Player)
            {
                playerNumbers.Add(i);
            }
        }

       return playerNumbers.IndexOf(battlerIndex);
    }

    int GetBattlerCPUNumber(int battlerIndex)
    {
       List<int> cpuNumbers = new List<int>();

        for (int i = 0; i < battlers.Count; i++)
        {
            if(battlers[i].aiType != AIType.Player)
            {
                cpuNumbers.Add(i);
            }
        }

       return cpuNumbers.IndexOf(battlerIndex);
    }

    List<int> CalculateTurnOrder()
    {
        List<int> participants = new List<int>();

        //add all battlers to turn order
        for (int i = 0; i < battlers.Count; i++)
        {
            if(battlers[i].lifePoints > 0)
            {
                participants.Add(i);
            }
            
        }

        participants = participants.OrderBy(x => battlers[x].gravityPoints).ThenBy(x => x == battleInitiator ? 0 : 1).ToList();



        return participants;
    }

    IEnumerator SetupBattle()
    {
        SetupBattlers();
        yield return StartCoroutine(SlideBattlerInfo(Team.Player, true));
        yield return StartCoroutine(SlideBattlerInfo(Team.Enemy, true));

        BattleAnnouncer.Instance.AnnounceGameStart();
        yield return StartCoroutine(DisplayPhasePopup(BattlePhase.START));
        yield return StartCoroutine(SpawnField());
        yield return StartCoroutine(ForceSetFieldChip());
    }

    IEnumerator ForceSetFieldChip()
    {
        for (int i = 0; i < battlers.Count; i++)
        {
           yield return StartCoroutine(chipSelect.MenuRoutine(i, ChipType.Field, false));
        }

        chipSelect.returnType = -1;
        yield return null;
    }

    public IEnumerator DisplayPhasePopup(BattlePhase phase)
    {
        GameObject popup = Instantiate(phasePopups[(int)phase], GameObject.Find("BattleCanvas").transform);
        Image popupImage = popup.GetComponent<Image>();


        popup.transform.DOScale(1, 0.2f);
        popupImage.DOFade(1, 0.2f);
        yield return new WaitForSeconds(1.3f);
        popup.transform.DOScale(0, 0.5f);
        popupImage.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);

        Destroy(popup, 1);
        yield return null;
    }

    public void CameraFocusTile(int tileIndex)
    {
        CameraManager.Instance.Target = fieldTiles[tileIndex].transform;
    }


    void SetupBattlers()
    {
        //set all battlers LP to 500
        for (int i = 0; i < battlers.Count; i++)
        {
            battlerInfo[i].battlerIndex = i;
            battlers[i].lifePoints = 500;
            
            for (int j = 0; j < battlers[i].cybeasts.Count; j++)
            {
                battlers[i].cybeasts[j].owner = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum BattlePhase
{
    START,
    SET,
    SKIRMISHSTART,
}