using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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
    }

    private void Start()
    {
        UpdateStatsUI();
        UpdateHealthUI();
    }

    private void OnEndTurnClicked()
    {
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
}
