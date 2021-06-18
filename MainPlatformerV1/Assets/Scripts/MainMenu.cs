using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;

    private readonly List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void StartGame(){
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("SceneA"));
        // scenesToLoad.Add(SceneManager.LoadSceneAsync("SceneB"));
        StartCoroutine(LoadingScreen());
    }
    public void HideMenu() => menu.SetActive(false);

    public void ShowLoadingScreen(){
        loadingInterface.SetActive(true);
    }
    private IEnumerator LoadingScreen(){
        float totalProgress = 0;
        foreach (var asyncOperation in scenesToLoad) {
            while (!asyncOperation.isDone) {
                totalProgress += asyncOperation.progress;
                loadingProgressBar.fillAmount = totalProgress / scenesToLoad.Count;
                yield return null;
            }
        }
    }

    public void ExitGame() => Application.Quit();

}
