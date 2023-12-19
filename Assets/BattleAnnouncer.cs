using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnnouncer : MonoBehaviour
{
    public AudioClip GameStart;
    public AudioClip[] PlayerTurn;
    public AudioClip[] CPUTurn;
    public AudioClip Set;
    public AudioClip SkirmishStart;
    
    public AudioSource audioSource;

    public static BattleAnnouncer Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AnnounceGameStart()
    {
        audioSource.PlayOneShot(GameStart);
    }

    public void AnnouncePlayerTurn(int player)
    {
        audioSource.PlayOneShot(PlayerTurn[player]);
    }

    public void AnnounceCPUTurn(int cpu)
    {
        audioSource.PlayOneShot(CPUTurn[cpu]);
    }

    public void AnnounceSet()
    {
        audioSource.PlayOneShot(Set);
    }

    public void AnnounceSkirmishStart()
    {
        audioSource.PlayOneShot(SkirmishStart);
    }
}
