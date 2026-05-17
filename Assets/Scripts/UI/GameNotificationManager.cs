using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameNotificationManager : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement container;
    private Label label;
    private Coroutine activeCoroutine;

    private void Awake()
    {
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

    private void HandleTurnChange()
    {
        int currentTurn = TurnManager.Instance.GetTurn();
        if (currentTurn == 0)
        {
            ShowNotification("YOUR TURN", "#61A2FF");
        }
        else if (currentTurn == 1)
        {
            ShowNotification("ENEMY TURN", "#FF4C4C");
        }
    }

    private void ShowNotification(string text, string hexColor)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
        activeCoroutine = StartCoroutine(NotificationFlow(text, hexColor));
    }

    private IEnumerator NotificationFlow(string text, string hexColor)
    {
        label.text = text;
        if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
        {
            label.style.color = color;
        }

        container.AddToClassList("visible");
        
        yield return new WaitForSeconds(1.2f);
        
        container.RemoveFromClassList("visible");
        activeCoroutine = null;
    }
}
