using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image _plane;
    [SerializeField] private Sprite[] _planeSprites;

    [SerializeField] private Image fadeImage;
    private float fadeDuration = 0.5f;

    [SerializeField] private GameObject[] _tutorialWindows;
    private int _windowIndex = 0;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _increaseSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        string audioCheck = PlayerPrefs.GetString("AudioCheck", "unMute");
        if (audioCheck == "unMute") AudioListener.volume = 1;
        else AudioListener.volume = 1;

        int skinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);
        _plane.sprite = _planeSprites[skinIndex];

        StartCoroutine(FadeIn());
    }

    public void LoadSceneWithFade(string sceneName)
    {
        PlayClickSound();
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void OpenNextWindow()
    {
        int activeWindow = 0;
        foreach (var item in _tutorialWindows)
        {
            if (item.activeInHierarchy)
            {
                activeWindow++;
            }
        }

        if (activeWindow == 0)
        {
            _tutorialWindows[_windowIndex].SetActive(true);
            PlayClickSound();
            return;
        }

        _tutorialWindows[_windowIndex].SetActive(false);
        _windowIndex++;
        if (_windowIndex >= 3)
        {
            PlayerPrefs.SetInt("FirstEnterCheck", 1);
            foreach (var item in _tutorialWindows)
            {
                item.SetActive(false);
            }
            _windowIndex = 0;
            return;
        }
        _tutorialWindows[_windowIndex].SetActive(true);
        PlayClickSound();
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
        Vibration.VibrateIOS(ImpactFeedbackStyle.Light);
    }

    public void PlayIncreaseSound()
    {
        _audioSource.PlayOneShot(_increaseSound);
        Vibration.VibrateIOS(ImpactFeedbackStyle.Rigid);
    }
}