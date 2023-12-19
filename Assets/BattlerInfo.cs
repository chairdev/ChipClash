using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class BattlerInfo : MonoBehaviour
{
    public int battlerIndex;
    public RectTransform anchor;
    public TextMeshProUGUI battlerName;
    public Image LPBar;
    public Image battlerLPBar;
    public IEnumerator Slide(bool slideIn)
    {
        if (slideIn)
        {
            anchor.DOAnchorPosX(51f, 0.5f);
        }
        else
        {
            anchor.DOAnchorPosX(-51f, 0.5f);
        }

        yield return new WaitForSeconds(0.5f);
    }

    void DrawUI()
    {
        battlerName.text = BattleManager.Instance.battlers[battlerIndex].name;
        battlerLPBar.transform.localScale = new Vector3((float)BattleManager.Instance.battlers[battlerIndex].lifePoints / 500f, 1, 1);

        if (BattleManager.Instance.battlers[battlerIndex].lifePoints > 250)
        {
            SetLPBarColor(Color.Lerp(Color.yellow, Color.green, (float)BattleManager.Instance.battlers[battlerIndex].lifePoints / 500f));
        }
        else
        {
            SetLPBarColor(Color.Lerp(Color.red, Color.yellow, (float)BattleManager.Instance.battlers[battlerIndex].lifePoints / 500f));
        }
    }

    void SetLPBarColor(Color color)
    {
        LPBar.color = color;
        battlerLPBar.color = color;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawUI();
    }
}
