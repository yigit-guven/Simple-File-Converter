using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemedUI : MonoBehaviour
{
    public enum UIType { Image, Text, TMP_Text }
    public enum ThemeColorRole { Background, Button, Text, Accent , OtherDark , OtherLight , Panel}

    public UIType type;
    public ThemeColorRole role;

    private void OnEnable()
    {
        ThemeManager.OnThemeChanged += ApplyTheme;
    }

    private void OnDisable()
    {
        ThemeManager.OnThemeChanged -= ApplyTheme;
    }

    private void Start()
    {
        if (ThemeManager.Instance != null)
        {
            ApplyTheme(ThemeManager.Instance.themes[ThemeManager.Instance.CurrentThemeIndex]);
        }
    }

    private void ApplyTheme(ThemeData theme)
    {
        Color selectedColor = GetColorForRole(theme);

        switch (type)
        {
            case UIType.Image:
                GetComponent<Image>().color = selectedColor;
                break;
            case UIType.Text:
                GetComponent<Text>().color = selectedColor;
                break;
            case UIType.TMP_Text:
                GetComponent<TMP_Text>().color = selectedColor;
                break;
        }
    }

    private Color GetColorForRole(ThemeData theme)
    {
        switch (role)
        {
            case ThemeColorRole.Background: return theme.backgroundColor;
            case ThemeColorRole.Button: return theme.buttonColor;
            case ThemeColorRole.Text: return theme.textColor;
            case ThemeColorRole.Accent: return theme.accentColor;
            case ThemeColorRole.OtherDark: return theme.otherDarkColor;
            case ThemeColorRole.OtherLight: return theme.otherLightColor;
            case ThemeColorRole.Panel: return theme.panelColor;
            default: return Color.white;
        }
    }
}
