using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BattleChipSelect : MonoBehaviour
{
    public int battlerIndex;
    public int currentSelection;
    public int currentTile;
    public int returnType;
    List<int> validChips = new List<int>();
    public ChipType chipType;
    public bool inMenu;

    public RectTransform anchor;
    public TextMeshProUGUI chipName;
    public TextMeshProUGUI chipDescription;

    public IEnumerator MenuRoutine(int battlerIndex, ChipType chipType, bool cancelable = true, bool isSkirmish = false)
    {
        this.battlerIndex = battlerIndex;
        this.chipType = chipType;

        currentSelection = 0;
        currentTile = 0;
        returnType = -1;

        validChips = BattleManager.Instance.battlers[this.battlerIndex].GetValidChips(chipType);

        switch(chipType)
        {
            case ChipType.Field:
                DrawFieldChip();
            break;
            case ChipType.Ability:
                DrawAbilityChip();
            break;
        }
        

        inMenu = true;

        anchor.DOAnchorPosY(0, 0.2f);
        yield return new WaitForSeconds(0.2f);

        while (inMenu)
        {
            if(InputManager.Instance.leftButtonGet)
            {
                currentSelection--;
                yield return new WaitForSeconds(0.2f);
            }
            else if(InputManager.Instance.rightButtonGet)
            {
                currentSelection++;
                yield return new WaitForSeconds(0.2f);
            }

            currentSelection = Mathf.Clamp(currentSelection, 0, validChips.Count - 1);

            switch(chipType)
            {
                case ChipType.Field:
                    DrawFieldChip();
                break;
                case ChipType.Ability:
                    DrawAbilityChip();
                break;
            }
        

            if(InputManager.Instance.aButtonDown)
            {
                if(!isSkirmish)
                {
                    yield return StartCoroutine(SelectTile());
                    if(BattleManager.Instance.fieldTiles[currentTile].chipOwner == battlerIndex)
                    {
                        inMenu = false;
                        returnType = 1;
                    }
                }
                else
                {

                }
                
            }
            else if(InputManager.Instance.bButtonDown && cancelable)
            {
                inMenu = false;
                returnType = 0;
            }

            yield return null;
        }

        anchor.DOAnchorPosY(-80, 0.2f);
        yield return new WaitForSeconds(0.2f);
    }

    public IEnumerator SelectTile(int mode = 0)
    {
        bool tileSelected = false;
        yield return new WaitForSeconds(0.2f);

        while(!tileSelected)
        {
            if(InputManager.Instance.leftButtonGet)
            {
                currentTile--;
                yield return new WaitForSeconds(0.2f);
            }
            else if(InputManager.Instance.rightButtonGet)
            {
                currentTile++;
                yield return new WaitForSeconds(0.2f);
            }
            else if(InputManager.Instance.upButtonGet)
            {
                currentTile -= 3;
                yield return new WaitForSeconds(0.2f);
            }
            else if(InputManager.Instance.downButtonGet)
            {
                currentTile += 3;
                yield return new WaitForSeconds(0.2f);
            }

            currentTile = Mathf.Clamp(currentTile, 0, 8);
            BattleManager.Instance.CameraFocusTile(currentTile);

            DrawTileSelection();


            if(InputManager.Instance.aButtonDown)
            {
                switch(mode)
                {
                    case 0:
                        if(BattleManager.Instance.fieldTiles[currentTile].chip == FieldChipList.None && BattleManager.Instance.fieldTiles[currentTile].chipOwner == -1)
                        {
                            tileSelected = true;

                            FieldChipList chip = BattleManager.Instance.battlers[battlerIndex].GetFieldChip(validChips[currentSelection]);
                            BattleManager.Instance.battlers[battlerIndex].PlaceChip(ChipType.Field, validChips[currentSelection]);
                            BattleManager.Instance.battlers[battlerIndex].fieldChips[validChips[currentSelection]].tile = currentTile;
                            yield return StartCoroutine(BattleManager.Instance.fieldTiles[currentTile].SetChip(chip, battlerIndex));
                        }
                    break;
                    case 1:
                        if(BattleManager.Instance.fieldTiles[currentTile].chip != FieldChipList.None)
                        {
                            tileSelected = true;
                            
                        }
                    break;

                }
                

                BattleManager.Instance.CameraFocusTile(4);
            }

            yield return null;
        }

       yield return null;
    }

    void DrawTileSelection()
    {
        for(int i = 0; i < 9; i++)
        {
            if(i == currentTile)
            {
                BattleManager.Instance.fieldTiles[i].isFlashing = true;
            }
            else
            {
                BattleManager.Instance.fieldTiles[i].isFlashing = false;
            }
        }
    }

    void DrawFieldChip()
    {
        FieldChipList chip = BattleManager.Instance.battlers[battlerIndex].GetFieldChip(validChips[currentSelection]);
        chipName.text = ChipManager.Instance.fieldChips[(int)chip].chipName;
        chipDescription.text = ChipManager.Instance.fieldChips[(int)chip].description;
    }

    void DrawAbilityChip()
    {
        AbilityChipList chip = BattleManager.Instance.battlers[battlerIndex].GetAbilityChip(validChips[currentSelection]);
        chipName.text = ChipManager.Instance.abilityChips[(int)chip].chipName;
        chipDescription.text = ChipManager.Instance.abilityChips[(int)chip].description;
    }
}
