using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnter : MonoBehaviour
{
    public Image _redBar;
    private string _sceneName = "Menu";

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Vibration.Init();
        StartCoroutine(FillLoadImage());
    }

    private IEnumerator FillLoadImage()
    {
        float currentTime = 0;
        while (currentTime < 2.3f)
        {
            currentTime += Time.deltaTime;
            _redBar.fillAmount = Mathf.Lerp(0, 1, currentTime / 2.3f);
            yield return null;
        }
        Vibration.VibratePop();
        SceneManager.LoadScene(_sceneName);
    }
}