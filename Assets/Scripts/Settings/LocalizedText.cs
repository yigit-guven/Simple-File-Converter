using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [System.Serializable]
    public class Translation
    {
        public LanguageCode language;
        [TextArea]
        public string text;
    }

    public List<Translation> translations;

    private static List<LocalizedText> allLocalizedTexts = new List<LocalizedText>();

    private TextMeshProUGUI textComponent;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        allLocalizedTexts.Add(this);
    }

    void Start()
    {
        ApplyLanguage();
    }

    void OnDestroy()
    {
        allLocalizedTexts.Remove(this);
    }

    public void ApplyLanguage()
    {
        var currentLang = LanguageManager.Instance?.CurrentLanguage ?? LanguageCode.English;

        foreach (var t in translations)
        {
            if (t.language == currentLang)
            {
                textComponent.text = t.text;
                return;
            }
        }

        // Fallback
        if (translations.Count > 0)
            textComponent.text = translations[0].text;
    }

    public static void UpdateAll()
    {
        foreach (var lt in allLocalizedTexts)
            lt.ApplyLanguage();
    }
}
