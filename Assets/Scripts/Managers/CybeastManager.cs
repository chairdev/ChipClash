using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CybeastManager : MonoBehaviour
{
    public CybeastData[] cybeasts;

    public static CybeastManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
