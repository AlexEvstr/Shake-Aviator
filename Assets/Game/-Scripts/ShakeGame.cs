using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShakeGame : MonoBehaviour
{
    [SerializeField] private Image planeImage; // Самолет
    [SerializeField] private Image blackOverlayImage; // Черный фон
    [SerializeField] private TMP_Text coinsText; // Текст монет

    private float shakeThreshold = 1.5f; // Порог тряски
    private float planeMinScale = 0.5f; // Минимальный размер самолета
    private float planeMaxScale = 5f; // Максимальный размер самолета
    private float growthSpeed = 1.0f; // Скорость роста самолета
    private float shrinkSpeed = 1.0f; // Скорость уменьшения самолета
    private float baseCoinRate = 1f; // Базовая скорость начисления монет

    private int totalCoins; // Общее количество монет
    private Vector3 originalPlaneScale; // Оригинальный размер самолета

    private float currentPlaneScale; // Текущий масштаб самолета

    private bool isShaking = false; // Идет ли тряска
    private float coinTimer = 0f; // Таймер для начисления монет

    private GameManager _gameManager;


    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        UpdateCoinsUI();

        originalPlaneScale = new Vector3(planeMinScale, planeMinScale, 1f);
        currentPlaneScale = planeMinScale;

        // Устанавливаем начальные значения
        planeImage.transform.localScale = originalPlaneScale;
        UpdateOverlayAlpha();
    }

    private void Update()
    {
        DetectShake();

        if (isShaking)
        {
            // Плавный рост самолета
            currentPlaneScale = Mathf.Min(planeMaxScale, currentPlaneScale + growthSpeed * Time.deltaTime);

            // Начисление монет
            coinTimer += Time.deltaTime;
            float coinRate = baseCoinRate * (currentPlaneScale / planeMinScale); // Чем больше размер, тем быстрее начисление
            if (coinTimer >= 1f / coinRate)
            {
                totalCoins++;
                PlayerPrefs.SetInt("TotalCoins", totalCoins);
                UpdateCoinsUI();
                coinTimer = 0f;
            }
        }
        else
        {
            // Плавное возвращение самолета к исходному размеру
            currentPlaneScale = Mathf.Max(planeMinScale, currentPlaneScale - shrinkSpeed * Time.deltaTime);
            coinTimer = 0f; // Сбрасываем таймер, чтобы монеты не начислялись без тряски
        }

        // Применяем изменения масштаба самолета
        planeImage.transform.localScale = new Vector3(currentPlaneScale, currentPlaneScale, 1f);

        // Обновляем прозрачность фона пропорционально размеру самолета
        UpdateOverlayAlpha();
    }

    private void DetectShake()
    {
        // Получаем ускорение устройства
        Vector3 acceleration = Input.acceleration;

        // Проверяем порог тряски
        isShaking = acceleration.sqrMagnitude > shakeThreshold * shakeThreshold;
    }

    private void UpdateOverlayAlpha()
    {
        // Прозрачность фона пропорциональна текущему масштабу самолета
        float progress = (currentPlaneScale - planeMinScale) / (planeMaxScale - planeMinScale);
        float alpha = Mathf.Lerp(0.9f, 0f, progress); // 0.9 - начальная прозрачность, 0 - конечная
        blackOverlayImage.color = new Color(blackOverlayImage.color.r, blackOverlayImage.color.g, blackOverlayImage.color.b, alpha);
    }

    private void UpdateCoinsUI()
    {
        coinsText.text = totalCoins.ToString();
        _gameManager.PlayIncreaseSound();
    }
}
