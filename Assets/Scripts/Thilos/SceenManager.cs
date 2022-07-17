using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SceenManager : MonoBehaviour
{
    public static SceenManager instance;
    public static SceenManager getInstance()
    {
        return instance;
    }
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(TargetManager.onChangeEnemyCount == null)
        {
            TargetManager.onChangeEnemyCount = new UnityEngine.Events.UnityEvent<int>();
        }
        TargetManager.onChangeEnemyCount.AddListener(onEnemyKilled);
    }

    public void onEnemyKilled(int count)
    {
        if (count == 0 && TargetSpawner.getInstance().getWaveCount() == 0)
            StartCoroutine(winCounterFunc());
    }

    IEnumerator winCounterFunc()
    {
        yield return new WaitForSeconds(0.1f);
        if(TargetManager.getCount() == 0)
        {
            Win();
        }
    }
    
    public void Win()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
    }

    public void onEnemyToBase()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Death");
    }

}