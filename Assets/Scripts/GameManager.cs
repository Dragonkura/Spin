using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] GameObject uiStart;
    [SerializeField] GameObject uiEnd;
    [SerializeField] CollecableItemSpawner collecableItemSpawner;
    [SerializeField] BoundCircle boundCircle;
    public bool isGamePlaying;
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (instance == null) instance = this;
        else DestroyImmediate(this);
        uiStart.gameObject.SetActive(true);
    }
    public void StartGame()
    {
        //collecableItemSpawner.StartSpawn();
        isGamePlaying = true;
        boundCircle?.OnStartGame();
    }
    public void EndGame()
    {
        uiStart.SetActive(true);
        if (collecableItemSpawner != null) collecableItemSpawner.StopSpawn();
    }
}
