using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Code.Scripts.Audio;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private LifeValue playerLife;
    
    private UIDocument uiDocument;
    
    private Label healthValue;
    private VisualElement healthBarFill;
    private VisualElement healthBox;

    private Label strengthValue;
    private Label shieldValue;
    private Label handlimitValue;
    private Label deckValue;

    private VisualElement strengthBox;
    private VisualElement shieldBox;
    private VisualElement handlimitBox;
    private VisualElement deckBox;
    
    private Button endTurnButton;
    private Coroutine healthBarCoroutine;

    // Tutorial Panel elements
    private VisualElement tutorialOverlay;
    private Button startGameButton;
    private VisualElement sceneFadeOverlay;

    // Game Over Panel elements
    private VisualElement gameOverOverlay;
    private Label gameOverTitle;
    private Label gameOverSubtitle;
    private Button restartButton;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;
        
        var root = uiDocument.rootVisualElement;

        healthValue = root.Q<Label>("health-value");
        healthBarFill = root.Q<VisualElement>("health-bar-fill");
        healthBox = root.Q<VisualElement>("health-box");

        strengthValue = root.Q<Label>("strength-value");
        shieldValue = root.Q<Label>("shield-value");
        handlimitValue = root.Q<Label>("handlimit-value");
        deckValue = root.Q<Label>("deck-value");

        strengthBox = root.Q<VisualElement>("strength-box");
        shieldBox = root.Q<VisualElement>("shield-box");
        handlimitBox = root.Q<VisualElement>("handlimit-box");
        deckBox = root.Q<VisualElement>("deck-box");

        endTurnButton = root.Q<Button>("end-turn-button");

        // Bind tutorial elements
        tutorialOverlay = root.Q<VisualElement>("tutorial-overlay");
        startGameButton = root.Q<Button>("start-game-button");
        sceneFadeOverlay = root.Q<VisualElement>("scene-fade-overlay");

        // Bind Game Over elements
        gameOverOverlay = root.Q<VisualElement>("game-over-overlay");
        gameOverTitle = root.Q<Label>("game-over-title");
        gameOverSubtitle = root.Q<Label>("game-over-subtitle");
        restartButton = root.Q<Button>("restart-button");
    }

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateStatsUI;
        if (playerLife != null)
        {
            playerLife.OnValueChanged += UpdateHealthUI;
        }

        if (endTurnButton != null)
        {
            endTurnButton.clicked += OnEndTurnClicked;
        }

        if (startGameButton != null)
        {
            startGameButton.clicked += OnStartGameClicked;
        }

        if (restartButton != null)
        {
            restartButton.clicked += OnRestartClicked;
        }

        Boss.OnBossDeath += HandleVictoryUI;
        CardPlayer.OnPlayerDeath += HandleDefeatUI;
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateStatsUI;
        if (playerLife != null)
        {
            playerLife.OnValueChanged -= UpdateHealthUI;
        }

        if (endTurnButton != null)
        {
            endTurnButton.clicked -= OnEndTurnClicked;
        }

        if (startGameButton != null)
        {
            startGameButton.clicked -= OnStartGameClicked;
        }

        if (restartButton != null)
        {
            restartButton.clicked -= OnRestartClicked;
        }

        Boss.OnBossDeath -= HandleVictoryUI;
        CardPlayer.OnPlayerDeath -= HandleDefeatUI;
    }

    private void Start()
    {
        UpdateStatsUI();
        UpdateHealthUI();

        if (sceneFadeOverlay != null)
        {
            StartCoroutine(FadeInSceneRoutine(sceneFadeOverlay));
        }
    }

    private IEnumerator FadeInSceneRoutine(VisualElement overlay)
    {
        yield return new WaitForEndOfFrame();
        
        float elapsed = 0f;
        float duration = 1.2f; // Smooth 1.2s fade-in from black

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            overlay.style.opacity = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        overlay.style.opacity = 0f;
        overlay.style.display = DisplayStyle.None;
    }

    private void OnStartGameClicked()
    {
        if (startGameButton != null)
        {
            startGameButton.clicked -= OnStartGameClicked;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Turn_End_Click");
        }

        if (tutorialOverlay != null)
        {
            StartCoroutine(FadeOutTutorial(tutorialOverlay));
        }
    }

    private IEnumerator FadeOutTutorial(VisualElement overlay)
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            overlay.style.opacity = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        overlay.style.opacity = 0f;
        overlay.style.display = DisplayStyle.None;

        // CINEMATIC SPAWN: Shuffle and draw cards now that the game begins!
        if (Deck.Instance != null)
        {
            Deck.Instance.InitializeStartingDeck();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Player_Turn_Start");
        }

        if (GameNotificationManager.Instance != null)
        {
            GameNotificationManager.Instance.ShowBattleStart();
        }
    }

    private void OnEndTurnClicked()
    {
        if (TurnManager.Instance != null && TurnManager.Instance.IsGameOver) return;

        if (TurnManager.Instance != null && TurnManager.Instance.GetTurn() == 0)
        {
            TurnManager.Instance.NextTurn();
        }
    }

    private void UpdateHealthUI()
    {
        if (playerLife == null || healthValue == null) return;

        if (healthValue.text != playerLife.Value.ToString())
        {
            healthValue.text = playerLife.Value.ToString();
            
            if (healthBarFill != null)
            {
                float targetPct = Mathf.Clamp01((float)playerLife.Value / 100f) * 100f;
                if (healthBarCoroutine != null) StopCoroutine(healthBarCoroutine);
                healthBarCoroutine = StartCoroutine(LerpHealthBar(targetPct));
            }

            BumpElement(healthBox);
        }
    }

    private IEnumerator LerpHealthBar(float targetPct)
    {
        float currentPct = healthBarFill.style.width.value.value;
        // Si el valor inicial es 0 o inválido, arranca desde 100% visualmente rápido
        if (float.IsNaN(currentPct) || currentPct <= 0f) currentPct = 100f; 

        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float newPct = Mathf.Lerp(currentPct, targetPct, t);
            healthBarFill.style.width = new Length(newPct, LengthUnit.Percent);
            yield return null;
        }

        healthBarFill.style.width = new Length(targetPct, LengthUnit.Percent);
    }

    private void UpdateStatsUI()
    {
        if (StatManager.Instance == null || strengthValue == null) return;

        if (strengthValue.text != StatManager.Instance.currentStrength.ToString())
        {
            strengthValue.text = StatManager.Instance.currentStrength.ToString();
            BumpElement(strengthBox);
        }

        string shieldDisplay = $"{StatManager.Instance.activeShield} ({StatManager.Instance.currentShieldStat})";
        if (shieldValue.text != shieldDisplay)
        {
            shieldValue.text = shieldDisplay;
            BumpElement(shieldBox);
        }

        int handCount = CardPlayer.Instance != null ? CardPlayer.Instance.GetHandCount() : 0;
        string handDisplay = $"{handCount}/{StatManager.Instance.currentMaxHandSize}";
        if (handlimitValue.text != handDisplay)
        {
            handlimitValue.text = handDisplay;
            BumpElement(handlimitBox);
        }

        if (Deck.Instance != null)
        {
            string deckCount = Deck.Instance.DrawStack.Count.ToString();
            if (deckValue.text != deckCount)
            {
                deckValue.text = deckCount;
                BumpElement(deckBox);
            }
        }
    }

    private void BumpElement(VisualElement element)
    {
        if (element == null) return;
        StartCoroutine(BumpCoroutine(element));
    }

    private IEnumerator BumpCoroutine(VisualElement element)
    {
        element.AddToClassList("bump");
        yield return new WaitForSeconds(0.15f);
        element.RemoveFromClassList("bump");
    }

    private void HandleVictoryUI()
    {
        if (gameOverOverlay == null || gameOverTitle == null || gameOverSubtitle == null) return;

        gameOverTitle.text = "VICTORY!";
        if (ColorUtility.TryParseHtmlString("#FFD700", out Color goldColor))
        {
            gameOverTitle.style.color = goldColor;
        }
        gameOverSubtitle.text = "YOU HAVE DEFEATED THE DEVOURER AND TAMED THE TIDES!";

        ShowGameOverOverlay();
    }

    private void HandleDefeatUI()
    {
        if (gameOverOverlay == null || gameOverTitle == null || gameOverSubtitle == null) return;

        gameOverTitle.text = "DEFEAT";
        if (ColorUtility.TryParseHtmlString("#C0392B", out Color redColor))
        {
            gameOverTitle.style.color = redColor;
        }
        gameOverSubtitle.text = "THE MAREA HAS CLAIMED ANOTHER VICTIM. TRY AGAIN.";

        ShowGameOverOverlay();
    }

    private void ShowGameOverOverlay()
    {
        if (gameOverOverlay == null) return;
        gameOverOverlay.style.display = DisplayStyle.Flex;
        StartCoroutine(FadeInOverlay(gameOverOverlay));
    }

    private IEnumerator FadeInOverlay(VisualElement overlay)
    {
        float elapsed = 0f;
        float duration = 0.8f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            overlay.style.opacity = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
        overlay.style.opacity = 1f;
    }

    private void OnRestartClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Turn_End_Click");
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
