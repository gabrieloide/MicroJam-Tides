using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingTextManager : MonoBehaviour
{
    private static FloatingTextManager instance;

    public static FloatingTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("FloatingTextManager");
                instance = go.AddComponent<FloatingTextManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    public void Show(Vector3 worldPosition, string text, Color color)
    {
        GameObject textObj = new GameObject("FloatingText");
        textObj.transform.position = worldPosition;
        
        TextMeshPro tmpro = textObj.AddComponent<TextMeshPro>();
        tmpro.text = text;
        tmpro.color = color;
        tmpro.fontSize = 6;
        tmpro.alignment = TextAlignmentOptions.Center;
        tmpro.fontStyle = FontStyles.Bold;
        
        if (Camera.main != null)
        {
            textObj.transform.rotation = Camera.main.transform.rotation;
        }

        // Animation sequence
        textObj.transform.DOMoveY(worldPosition.y + 0.8f, 1.2f).SetEase(Ease.OutCubic);
        textObj.transform.DOScale(Vector3.one * 1.5f, 0.2f).OnComplete(() => {
            textObj.transform.DOScale(Vector3.one, 0.2f);
        });
        
        tmpro.DOFade(0, 1.2f).SetDelay(0.3f).OnComplete(() => Destroy(textObj));
    }
}
