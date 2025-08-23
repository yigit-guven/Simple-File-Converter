using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class FilePickerUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text fileNameText;
    public GameObject panel1;
    public GameObject panel2;
    public GameObject conversionPanel;   // panel where dropdown lives
    public TMP_Dropdown conversionDropdown;

    private string selectedExtension;

    public void PickFile()
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        string[] allowedExtensions = { "txt", "pdf", "docx", "xls", "xlsx" };

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("File picking cancelled.");
            }
            else
            {
                string fileName = Path.GetFileName(path);
                fileNameText.text = fileName;

                selectedExtension = Path.GetExtension(path).TrimStart('.').ToLower();

                panel1.SetActive(true);
                panel2.SetActive(true);

            }
        }, allowedExtensions);
    }

    public void ShowConversionOptions()
    {
        if (string.IsNullOrEmpty(selectedExtension))
        {
            Debug.LogWarning("No file selected yet!");
            return;
        }

        conversionDropdown.ClearOptions();
        List<string> options = new List<string>();

        if (ConversionMap.SupportedConversions.ContainsKey(selectedExtension))
        {
            foreach (string output in ConversionMap.SupportedConversions[selectedExtension])
            {
                options.Add($".{selectedExtension} -> .{output}");
            }
        }
        else
        {
            options.Add("No conversions available for ." + selectedExtension);
        }

        conversionDropdown.AddOptions(options);
        conversionPanel.SetActive(true);
    }

    // Get selected conversion
    public string GetSelectedConversion()
    {
        if (conversionDropdown.options.Count == 0)
            return null;

        return conversionDropdown.options[conversionDropdown.value].text;
    }
}
