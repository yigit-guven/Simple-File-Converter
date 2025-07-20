using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public LanguageCode CurrentLanguage { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            int savedLang = PlayerPrefs.GetInt("language", 0);
            CurrentLanguage = (LanguageCode)savedLang;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLanguage(LanguageCode lang)
    {
        CurrentLanguage = lang;
        PlayerPrefs.SetInt("language", (int)lang);
        PlayerPrefs.Save();

        LocalizedText.UpdateAll();
    }

    public void SetEnglish() => SetLanguage(LanguageCode.English);
    public void SetSpanish() => SetLanguage(LanguageCode.Spanish);
    public void SetDeutch() => SetLanguage(LanguageCode.Deutch);
    public void SetFrench() => SetLanguage(LanguageCode.French);
    public void SetPortuguese() => SetLanguage(LanguageCode.Portuguese);
    public void SetJapanese() => SetLanguage(LanguageCode.Japanese);
    public void SetItalian() => SetLanguage(LanguageCode.Italian);
    public void SetRussian() => SetLanguage(LanguageCode.Russian);
    public void SetTurkish() => SetLanguage(LanguageCode.Turkish);
}
