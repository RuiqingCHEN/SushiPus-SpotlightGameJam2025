using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ASyncManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private Slider loadingSlider;

    public void LoadBtn()
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        // Run the A sync
        StartCoroutine(LoadASync());
    }

    IEnumerator LoadASync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(1);

        while(!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }

}
