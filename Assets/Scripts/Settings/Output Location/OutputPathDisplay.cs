using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OutputPathDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text outputPathText;

    private static readonly Dictionary<LanguageCode, string> sameLocationTranslations = new Dictionary<LanguageCode, string>()
    {
        { LanguageCode.English, "Same as input file location" },
        { LanguageCode.Spanish, "Misma ubicación que el archivo de entrada" },
        { LanguageCode.Deutch, "Gleicher Speicherort wie Eingabedatei" },
        { LanguageCode.French, "Même emplacement que le fichier d'entrée" },
        { LanguageCode.Portuguese, "Mesmo local do arquivo de entrada" },
        { LanguageCode.Japanese, "入力ファイルと同じ場所" },
        { LanguageCode.Italian, "Stessa posizione del file di input" },
        { LanguageCode.Russian, "Та же папка, что и файл ввода" },
        { LanguageCode.Turkish, "Girdi dosyasıyla aynı konum" }
    };

    private void Start()
    {
        UpdateOutputPathText();
    }

    public void UpdateOutputPathText()
    {
        if (outputPathText == null)
            return;

        LanguageCode lang = LanguageCode.English;
        if (LanguageManager.Instance != null)
        {
            lang = LanguageManager.Instance.CurrentLanguage;
        }

        // Since we always use same as input now, just show the translation
        if (!sameLocationTranslations.TryGetValue(lang, out string displayPath))
        {
            displayPath = sameLocationTranslations[LanguageCode.English];
        }

        outputPathText.text = displayPath;
    }
}