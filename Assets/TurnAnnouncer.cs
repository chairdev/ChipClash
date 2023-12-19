using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TurnAnnouncer : MonoBehaviour
{
    public RectTransform anchor;
    public TextMeshProUGUI text;

    public IEnumerator AnnounceTurn(int battler)
    {
        yield return StartCoroutine(DoText(BattleManager.Instance.battlers[battler].name + "'s turn!"));
    }

    public IEnumerator DoText(string textToAnnounce)
    {
        text.text = textToAnnounce;
        //scale up horizontally first, then vertically
        anchor.DOScale(new Vector3(1f, 0.2f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        anchor.DOScale(new Vector3(1f, 1f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(1f);

        anchor.DOScale(new Vector3(1f, 0.2f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
        anchor.DOScale(new Vector3(0f, 0f, 1f), 0.2f);
        yield return new WaitForSeconds(0.2f);
    }
}
