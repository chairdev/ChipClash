using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TurnSelect : MonoBehaviour
{
    public int[] selection = new int[8];
    public bool inMenu;
    public int currentBattler;

    RectTransform anchor;
    public GameObject selectPrefab;
    public Sprite[] windows;

    void Start()
    {
        anchor = GetComponent<RectTransform>();
    }

    public IEnumerator Routine(int battlerIndex)
    {
        inMenu = true;
        currentBattler = battlerIndex;
        selection = new int[8];

        while(inMenu)
        {
            yield return StartCoroutine(DoSelection(new string[] { "Battle Chips", "Cybeast", "Field", "Pass"}, 0, false));
            switch (selection[0])
            {
                case 0:
                    yield return StartCoroutine(BattleChipMenu());
                break;
                case 1:
                    yield return StartCoroutine(CybeastMenu());
                break;
                case 2:
                break;
                case 3:
                    inMenu = false;
                break;
            }

            yield return null;
        }

        yield return null;
    }

    IEnumerator BattleChipMenu()
    {
        if (BattleManager.Instance.chipSelect.returnType == 1 || !BattleManager.Instance.battlers[currentBattler].CanPlaceAnyChips(ChipType.Field))
        {
            yield return StartCoroutine(DoSelection(new string[] { "Counter Chips" }, 1));
            switch (selection[1])
            {
                case 0:
                break;
            }
        }
        else
        {
            yield return StartCoroutine(DoSelection(new string[] { "Field Chips", "Counter Chips" }, 1));
            switch (selection[1])
            {
                case 0:
                    if (BattleManager.Instance.chipSelect.returnType != 1)
                    {
                        yield return StartCoroutine(BattleManager.Instance.chipSelect.MenuRoutine(currentBattler, ChipType.Field));
                    }
                    break;
                case 1:
                    // Add code for case 1 if needed
                    break;
            }
        }
    }

    IEnumerator CybeastMenu()
    {
        yield return StartCoroutine(BattleManager.Instance.chipSelect.SelectTile(1));

        BattleAnnouncer.Instance.AnnounceSet();

        bool isPlayer = BattleManager.Instance.battlers[currentBattler].aiType == AIType.Player;

        BattleManager.Instance.battlers[currentBattler].cybeasts[0].Set(BattleManager.Instance.chipSelect.currentTile);

        yield return StartCoroutine(BattleManager.Instance.DisplayCybeastsOnTile(BattleManager.Instance.chipSelect.currentTile));
        
        yield return StartCoroutine(BattleManager.Instance.DisplayPhasePopup(BattlePhase.SET));

        inMenu = false;
    }


    public IEnumerator DoSelection(string[] options, int stack, bool cancelable = true)
    {
        List<GameObject> selections = new List<GameObject>();
        List<Image> selectionImages = new List<Image>();
        List<TextMeshProUGUI> selectionTexts = new List<TextMeshProUGUI>();

        bool selected = false;


        for (int i = 0; i < options.Length; i++)
        {
            selections.Add(Instantiate(selectPrefab, anchor));
            selectionImages.Add(selections[i].GetComponent<Image>());
            selectionTexts.Add(selections[i].GetComponentInChildren<TextMeshProUGUI>());

            selectionTexts[i].text = options[i];
        }

        anchor.DOAnchorPosY(0, 0.2f);
        yield return new WaitForSeconds(0.2f);

        while (!selected)
        {
            if(InputManager.Instance.leftButtonGet)
            {
                selection[stack] = (selection[stack] + options.Length - 1) % options.Length;
                yield return new WaitForSeconds(0.1f);
            }
            else if(InputManager.Instance.rightButtonGet)
            {
                selection[stack] = (selection[stack] + 1) % options.Length;
                yield return new WaitForSeconds(0.1f);
            }
            else if(InputManager.Instance.aButtonDown)
            {
                selected = true;
            }
            else if(InputManager.Instance.bButtonDown && cancelable)
            {
                selection[stack] = -1;
                selected = true;
            }

            for (int i = 0; i < options.Length; i++)
            {
                if(i == selection[stack])
                {
                    selectionImages[i].sprite = windows[1];
                }
                else
                {
                    selectionImages[i].sprite = windows[0];
                }
            }

            yield return null;
        }


        anchor.DOAnchorPosY(-20, 0.2f);
        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < options.Length; i++)
        {
            Destroy(selections[i]);
        }
    }
}
