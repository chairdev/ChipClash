using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayBGM(BGM bgm)
    {
        if (bgm == BGM.NONE)
        {
            return;
        }
        audioSource.clip = audioClips[(int)bgm];
        audioSource.Play();
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

public enum BGM
{
    NONE = 0,
    NORMALARENA = 1,
    BATTLECONFRONTATION = 2,
}
