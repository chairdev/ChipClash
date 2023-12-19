using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldTile : MonoBehaviour
{
    public FieldChipList chip;
    public int chipOwner = -1;
    public bool isFlashing;
    public Image image;
    public Image chipImage;
    public RectTransform chipTransform;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            if (isFlashing)
            {
                image.DOFade(1f, 0.2f);
                yield return new WaitForSeconds(0.2f);
                image.DOFade(0f, 0.2f);
                yield return new WaitForSeconds(0.2f);
            }

            yield return null;
        }
    }

    public IEnumerator SetChip(FieldChipList chip, int chipOwner)
    {
        this.chip = chip;
        this.chipOwner = chipOwner;

        isFlashing = false;

        //chipImage.sprite = ChipManager.Instance.fieldChips[(int)chip].sprite;
        chipTransform.DOScale(new Vector2(1f, 1f), 0.2f);

        yield return new WaitForSeconds(0.2f);
    }

    public void DestroyChip()
    {
        chip = FieldChipList.None;
        chipOwner = -1;

        chipTransform.localScale = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
