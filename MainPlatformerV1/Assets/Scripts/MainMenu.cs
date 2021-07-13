using System.Collections;
using System.Collections.Generic;
using ThunderNut.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;

    private readonly List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    [SerializeField] private SceneHandle sceneHandle;

    public void StartGame(){
        HideMenu();
        ShowLoadingScreen();
        // scenesToLoad.Add(SceneManager.LoadSceneAsync($"{sceneHandle.scene.name}", LoadSceneMode.Additive));
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
