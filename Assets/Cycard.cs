using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cycard : MonoBehaviour
{
    public Cybeast beast;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerLevelText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(beast != null)
        {
            nameText.text = beast.GetName();
            powerLevelText.text = beast.powerLevel.ToString();
        }
        else
        {
            nameText.text = "";
            powerLevelText.text = "";
        }
    }
}
