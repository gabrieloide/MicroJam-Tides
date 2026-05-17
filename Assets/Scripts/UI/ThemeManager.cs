using UnityEngine;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    [Header("Font Assets")]
    public TMP_FontAsset titleFont;
    public TMP_FontAsset bodyFont;

    [Header("Settings")]
    public float titleSizeThreshold = 35f;

    [ContextMenu("Apply Fonts to Scene")]
    public void ApplyFontsToScene()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>(true);
        int count = 0;

        foreach (var text in allTexts)
        {
            if (text.fontSize >= titleSizeThreshold)
            {
                text.font = titleFont;
            }
            else
            {
                text.font = bodyFont;
            }
            
            // Opcional: Desactivar antialiasing o ajustar para pixel art
            text.extraPadding = true;
            
            count++;
        }

        Debug.Log($"[ThemeManager] Applied fonts to {count} text elements.");
    }
}
