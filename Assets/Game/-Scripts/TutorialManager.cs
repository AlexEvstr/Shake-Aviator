using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuWindows;
    [SerializeField] private GameObject[] _tutorialWindows;
    private int _windowIndex = 0;

    private void Start()
    {
        int FirstEnter = PlayerPrefs.GetInt("FirstEnterCheck", 0);
        if (FirstEnter == 0)
        {
            _menuWindows[1].SetActive(false);
            _menuWindows[0].SetActive(true);
        }
        else
        {
            _menuWindows[0].SetActive(false);
            _menuWindows[1].SetActive(true);
        }
    }

    public void OpenNextWindow()
    {
        _tutorialWindows[_windowIndex].SetActive(false);
        _windowIndex++;
        if (_windowIndex >= 3)
        {
            _menuWindows[0].SetActive(false);
            _menuWindows[1].SetActive(true);
            PlayerPrefs.SetInt("FirstEnterCheck", 1);
            return;
        }
        _tutorialWindows[_windowIndex].SetActive(true);
    }
}
