using System;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public int currentTime { get; private set; }
    public int maxTime { get { return maxTimeSetterInSeconds; } }
    public Action OneSecondPassed;
    [SerializeField] SQLConnection sqlConnector;
    [SerializeField] int maxTimeSetterInSeconds;
    //bool inGame;
    private void Start()
    {
        sqlConnector.signedIn += Play;
    }

    public void Play()
    {
        //inGame = true;
        InvokeRepeating("UpdateStates", 0, 1);
    }

    void UpdateStates()
    {
        currentTime += 1;
        OneSecondPassed?.Invoke();
    }
}
