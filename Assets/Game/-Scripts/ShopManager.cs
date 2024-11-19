using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class Skin
    {
        public Image buttonImage;
    }

    public Skin[] skins;
    public Sprite[] planeSprites;
    public int skinCost = 350;
    public TMP_Text totalCoinsText;
    public Image planeImage;
    public Sprite[] buttonSprites;

    private int totalCoins;
    private int selectedSkinIndex = 0;
    private MenuManager _menuManager;
    [SerializeField] private GameObject _notEnough;

    private void Start()
    {
        _menuManager = GetComponent<MenuManager>();
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinsUI();

        LoadSkinsState();

        UpdateSkinButtons();
        planeImage.sprite = planeSprites[selectedSkinIndex];
    }

    public void OnSkinButtonClick(int index)
    {
        if (!IsSkinBought(index))
        {
            if (totalCoins >= skinCost)
            {
                totalCoins -= skinCost;
                PlayerPrefs.SetInt("TotalCoins", totalCoins);
                UpdateCoinsUI();

                SetSkinBought(index);

                SetSelectedSkin(index);
                _menuManager.PlaySuccessSound();
            }
            else
            {
                StartCoroutine(ShowNotEnough());
                _menuManager.PlayErrorSound();
            }
        }
        else
        {
            SetSelectedSkin(index);
            _menuManager.PlayClickSound();
        }
    }

    private void SetSelectedSkin(int selectedIndex)
    {
        selectedSkinIndex = selectedIndex;
        PlayerPrefs.SetInt("SelectedSkin", selectedSkinIndex);

        UpdateSkinButtons();

        planeImage.sprite = planeSprites[selectedSkinIndex];
    }

    private void UpdateSkinButtons()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            if (i == selectedSkinIndex)
            {
                skins[i].buttonImage.sprite = buttonSprites[2];
            }
            else
            {
                skins[i].buttonImage.sprite = IsSkinBought(i) ? buttonSprites[1] : buttonSprites[0];
            }
        }
    }

    private void UpdateCoinsUI()
    {
        totalCoinsText.text = totalCoins.ToString();
    }

    private bool IsSkinBought(int index)
    {
        return PlayerPrefs.GetInt($"SkinBought_{index}", 0) == 1;
    }

    private void SetSkinBought(int index)
    {
        PlayerPrefs.SetInt($"SkinBought_{index}", 1);
    }

    private void LoadSkinsState()
    {
        selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkin", 0);

        if (!IsSkinBought(0))
        {
            SetSkinBought(0);
        }
    }

    private IEnumerator ShowNotEnough()
    {
       if (!_notEnough.activeInHierarchy)
        {
            _notEnough.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            _notEnough.SetActive(false);
        }
    }
}