using UnityEngine;
using TMPro;
using System.IO;

public class FilePickerController : MonoBehaviour
{
    [SerializeField] private TMP_Text fileNameText;
    [SerializeField] private ConversionSelector conversionSelector;

    private string currentFilePath = "";
    private readonly string[] allowedFileTypes = new string[] { "txt", "pdf", "csv", "json", "xml" };

    private void Start()
    {
        SetNotSelectedText();
    }

    public void OpenFilePicker()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        NativeFilePicker.Permission permission = NativeFilePicker.CheckPermission();
        if (permission != NativeFilePicker.Permission.Granted)
        {
            permission = NativeFilePicker.RequestPermission();
            if (permission != NativeFilePicker.Permission.Granted)
            {
                Debug.LogWarning("Permission denied.");
                SetNotSelectedText();
                return;
            }
        }
#endif

        if (NativeFilePicker.IsFilePickerBusy())
            return;

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                SetNotSelectedText();
                conversionSelector.gameObject.SetActive(false);
            }
            else
            {
                currentFilePath = path;
                string fileName = Path.GetFileName(path);
                fileNameText.text = fileName;
                conversionSelector.ShowConversionOptions(path);
            }
        }, allowedFileTypes);
    }

    public string GetSelectedFilePath()
    {
        return string.IsNullOrEmpty(currentFilePath) ? null : currentFilePath;
    }

    private void SetNotSelectedText()
    {
        if (fileNameText == null) return;
        currentFilePath = "";
        fileNameText.text = GetLocalizedNotSelected();
    }

    private string GetLocalizedNotSelected()
    {
        switch (LanguageManager.Instance.CurrentLanguage)
        {
            case LanguageCode.Turkish: return "Dosya seçilmedi";
            case LanguageCode.Spanish: return "Archivo no seleccionado";
            case LanguageCode.Deutch: return "Keine Datei ausgewählt";
            case LanguageCode.French: return "Aucun fichier sélectionné";
            case LanguageCode.Portuguese: return "Nenhum arquivo selecionado";
            case LanguageCode.Japanese: return "ファイルが選択されていません";
            case LanguageCode.Italian: return "Nessun file selezionato";
            case LanguageCode.Russian: return "Файл не выбран";
            case LanguageCode.English:
            default: return "No file selected";
        }
    }
}
