using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickPlayAudio : MonoBehaviour
{
    [SerializeField] string AudioName;

    private void Awake()
    {
        FindObjectOfType<AudioManager>().Play(AudioName);
    }

    public void EventQuickPlay() 
    {
        FindObjectOfType<AudioManager>().Play(AudioName);
    }
}
