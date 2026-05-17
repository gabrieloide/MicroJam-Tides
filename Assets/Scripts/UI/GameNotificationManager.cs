using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameNotificationManager : MonoBehaviour
{
    public static GameNotificationManager Instance { get; private set; }

    private UIDocument uiDocument;
    private VisualElement container;
    private Label label;
    private Coroutine activeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        container = root.Q<VisualElement>("notification-container");
        label = root.Q<Label>("notification-label");
        
        container.RemoveFromClassList("visible");
    }

    private void OnEnable()
    {
        TurnManager.OnTurnChange += HandleTurnChange;
    }

    private void OnDisable()
    {
        TurnManager.OnTurnChange -= HandleTurnChange;
    }

    private void Start()
    {
        ShowBattleStart();
    }

    private void HandleTurnChange()
    {
        int currentTurn = TurnManager.Instance.GetTurn();
        if (currentTurn == 0)
        {
            ShowNotification("YOUR TURN", "#61A2FF", 1.2f);
        }
        else if (currentTurn == 1)
        {
            ShowNotification("ENEMY TURN", "#FF8C00", 1.2f);
        }
    }

    public void ShowBattleStart()
    {
        ShowNotification("FIGHT!", "#51A200", 2f);
    }

    public void ShowVictory()
    {
        ShowNotification("VICTORY!", "#FFD700", 5f);
    }

    public void ShowDefeat()
    {
        ShowNotification("DEFEAT", "#333333", 5f);
    }

    private void ShowNotification(string text, string hexColor, float duration)
    {
        if (container == null || label == null) return;
        
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(NotificationFlow(text, hexColor, duration));
    }

    private IEnumerator NotificationFlow(string text, string hexColor, float duration)
    {
        label.text = text;
        if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
        {
            label.style.color = color;
        }

        container.AddToClassList("visible");
        
        yield return new WaitForSeconds(duration);
        
        container.RemoveFromClassList("visible");
        activeCoroutine = null;
    }
}
