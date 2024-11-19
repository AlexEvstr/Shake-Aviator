using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _audioBtns;
    private AudioSource _audioSource;
    public Image fadeImage;
    private float fadeDuration = 0.5f;

    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _errorSound;
    [SerializeField] private AudioClip _succesSound;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        string audioCheck = PlayerPrefs.GetString("AudioCheck", "unMute");
        if (audioCheck == "unMute")
        {
            _audioBtns[1].SetActive(false);
            _audioBtns[0].SetActive(true);
            _audioSource.volume = 1;
        }
        else
        {
            _audioBtns[0].SetActive(false);
            _audioBtns[1].SetActive(true);
            _audioSource.volume = 0;
        }

        fadeImage.color = new Color(0, 0, 0, 1);

        StartCoroutine(FadeIn());
    }

    public void DisableAidio()
    {
        _audioBtns[0].SetActive(false);
        _audioBtns[1].SetActive(true);
        _audioSource.volume = 0;
        PlayerPrefs.SetString("AudioCheck", "Mute");
        PlayClickSound();
    }

    public void EnableAidio()
    {
        _audioBtns[1].SetActive(false);
        _audioBtns[0].SetActive(true);
        _audioSource.volume = 1;
        PlayerPrefs.SetString("AudioCheck", "unMute");
        PlayClickSound();
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

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
        Vibration.VibrateIOS(ImpactFeedbackStyle.Light);
    }

    public void PlayErrorSound()
    {
        _audioSource.PlayOneShot(_errorSound);
        Vibration.VibrateIOS(NotificationFeedbackStyle.Error);
    }

    public void PlaySuccessSound()
    {
        _audioSource.PlayOneShot(_succesSound);
        Vibration.VibrateIOS(NotificationFeedbackStyle.Success);
    }
}