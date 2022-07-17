using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DDestroyOnLoad : MonoBehaviour
{
    public string NextLevelName;
    public string ThisLevelName;

    AudioSource player;

    public AudioClip bgm;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip level;

    public enum variants
    {
        bgm,
        win,
        lose,
        level
    }

    public static DDestroyOnLoad instance;
    public static DDestroyOnLoad getInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    public void StartPlaying(variants v)
    {
        switch (v)
        {
            case variants.bgm:
                player.clip = bgm;
                player.loop = true;
                break;
            case variants.win:
                player.clip = win;
                player.loop = false;
                break;
            case variants.lose:
                player.clip = lose;
                player.loop = false;
                break;
            case variants.level:
                player.clip = level;
                player.loop = true;
                break;
        }
        player.Play();
    }
    public void StopPlaying()
    {
        player.Stop();
    }

    void Start()
    {
        player = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);
    }
}