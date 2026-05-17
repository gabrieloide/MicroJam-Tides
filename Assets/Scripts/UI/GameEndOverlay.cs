using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameEndOverlay : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private CanvasGroup defeatPanel;

    [Header("Buttons")]
    [SerializeField] private Button restartButtonVictory;
    [SerializeField] private Button restartButtonDefeat;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.alpha = 0f;
            victoryPanel.interactable = false;
            victoryPanel.blocksRaycasts = false;
            victoryPanel.gameObject.SetActive(false);
        }

        if (defeatPanel != null)
        {
            defeatPanel.alpha = 0f;
            defeatPanel.interactable = false;
            defeatPanel.blocksRaycasts = false;
            defeatPanel.gameObject.SetActive(false);
        }

        if (restartButtonVictory != null)
        {
            restartButtonVictory.onClick.AddListener(RestartGame);
        }

        if (restartButtonDefeat != null)
        {
            restartButtonDefeat.onClick.AddListener(RestartGame);
        }

        Boss.OnBossDeath += ShowVictory;
        CardPlayer.OnPlayerDeath += ShowDefeat;
    }

    private void OnDestroy()
    {
        Boss.OnBossDeath -= ShowVictory;
        CardPlayer.OnPlayerDeath -= ShowDefeat;
    }

    private void ShowVictory()
    {
        if (victoryPanel == null) return;

        victoryPanel.gameObject.SetActive(true);
        
        victoryPanel.DOFade(1f, 0.8f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                victoryPanel.interactable = true;
                victoryPanel.blocksRaycasts = true;
            });
    }

    private void ShowDefeat()
    {
        if (defeatPanel == null) return;

        defeatPanel.gameObject.SetActive(true);

        defeatPanel.DOFade(1f, 0.8f)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                defeatPanel.interactable = true;
                defeatPanel.blocksRaycasts = true;
            });
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
