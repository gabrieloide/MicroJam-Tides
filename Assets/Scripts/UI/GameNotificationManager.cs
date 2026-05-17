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
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument != null && uiDocument.rootVisualElement != null)
        {
            var root = uiDocument.rootVisualElement;
            container = root.Q<VisualElement>("notification-container");
            label = root.Q<Label>("notification-label");
            
            if (container != null)
            {
                container.style.opacity = 0f;
                container.style.display = DisplayStyle.None;
            }
        }
    }

    private void HandleTurnChange()
    {
        int currentTurn = TurnManager.Instance.GetTurn();
        if (currentTurn == 0)
        {
            ShowNotification("YOUR TURN", "#61A2FF", 2f);
        }
    }
 
    public void ShowEnemyTurn()
    {
        ShowNotification("ENEMY TURN", "#FF8C00", 2f);
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

        container.style.display = DisplayStyle.Flex;
        
        float elapsed = 0f;
        float fadeTime = 0.3f;
        while(elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;
            
            float scale = Mathf.Lerp(0.5f, 1.0f, Mathf.Sin(t * Mathf.PI * 0.6f));
            
            container.style.opacity = Mathf.Lerp(0f, 1f, t);
            container.style.scale = new StyleScale(new Scale(new Vector3(scale, scale, 1f)));
            container.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(Mathf.Lerp(-30, 0, t), LengthUnit.Pixel), 0));
            yield return null;
        }

        container.style.opacity = 1f;
        container.style.scale = new StyleScale(new Scale(Vector3.one));
        container.style.translate = new StyleTranslate(new Translate(new Length(0, LengthUnit.Pixel), new Length(0, LengthUnit.Pixel), 0));

        yield return new WaitForSeconds(duration);
        
        elapsed = 0f;
        while(elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;
            container.style.opacity = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        
        container.style.opacity = 0f;
        container.style.display = DisplayStyle.None;
        activeCoroutine = null;
    }
}
