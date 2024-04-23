using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private SignalBus signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
        signalBus.Subscribe<NextSceneSignal>(LoadNextLevel);
    }
    private void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        int maxIndex = SceneManager.sceneCountInBuildSettings;
        Debug.Log(maxIndex);

        if (sceneIndex >= maxIndex)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(++sceneIndex);
        }
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<NextSceneSignal>(LoadNextLevel);
    }
}
public class NextSceneSignal { }
