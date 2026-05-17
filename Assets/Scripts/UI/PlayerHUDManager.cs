using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUDManager : MonoBehaviour
{
    [SerializeField] private LifeValue playerLife;
    
    private UIDocument uiDocument;
    
    private Label healthValue;
    private Label strengthValue;
    private Label shieldValue;
    private Label handlimitValue;
    private Label deckValue;

    private VisualElement healthBox;
    private VisualElement strengthBox;
    private VisualElement shieldBox;
    private VisualElement handlimitBox;
    private VisualElement deckBox;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;
        
        var root = uiDocument.rootVisualElement;

        healthValue = root.Q<Label>("health-value");
        strengthValue = root.Q<Label>("strength-value");
        shieldValue = root.Q<Label>("shield-value");
        handlimitValue = root.Q<Label>("handlimit-value");
        deckValue = root.Q<Label>("deck-value");

        healthBox = root.Q<VisualElement>("health-box");
        strengthBox = root.Q<VisualElement>("strength-box");
        shieldBox = root.Q<VisualElement>("shield-box");
        handlimitBox = root.Q<VisualElement>("handlimit-box");
        deckBox = root.Q<VisualElement>("deck-box");
    }

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateStatsUI;
        if (playerLife != null)
        {
            playerLife.OnValueChanged += UpdateHealthUI;
        }
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateStatsUI;
        if (playerLife != null)
        {
            playerLife.OnValueChanged -= UpdateHealthUI;
        }
    }

    private void Start()
    {
        UpdateStatsUI();
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (playerLife == null || healthValue == null) return;

        if (healthValue.text != playerLife.Value.ToString())
        {
            healthValue.text = playerLife.Value.ToString();
            BumpElement(healthBox);
        }
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
