using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class ConversionSelector : MonoBehaviour
{
    [SerializeField] private GameObject conversionPanel;
    [SerializeField] private TMP_Dropdown conversionDropdown;

    public UnityEvent<bool> OnConversionTypeSelected; // true = valid selection

    private Dictionary<string, List<string>> conversionMap = new Dictionary<string, List<string>>()
    {
        { ".txt", new List<string> { "pdf", "csv" } },
        { ".csv", new List<string> { "txt", "json" } },
        { ".json", new List<string> { "csv", "txt" } },
        { ".xml", new List<string> { "json", "txt" } },
        { ".pdf", new List<string> { "txt" } }
    };

    void Start()
    {
        conversionPanel.SetActive(false);

        // Listen for dropdown changes
        conversionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void ShowConversionOptions(string inputFilePath)
    {
        string extension = System.IO.Path.GetExtension(inputFilePath).ToLower();

        conversionPanel.SetActive(true);

        conversionDropdown.ClearOptions();

        if (conversionMap.ContainsKey(extension))
        {
            var options = conversionMap[extension];
            conversionDropdown.AddOptions(options);
            conversionDropdown.interactable = true;
            OnConversionTypeSelected?.Invoke(true);
        }
        else
        {
            conversionDropdown.AddOptions(new List<string> { "No conversion available" });
            conversionDropdown.interactable = false;
            OnConversionTypeSelected?.Invoke(false);
        }
    }

    private void OnDropdownValueChanged(int index)
    {
        // If dropdown is interactable, valid selection
        bool valid = conversionDropdown.interactable && conversionDropdown.options.Count > 0;
        OnConversionTypeSelected?.Invoke(valid);
    }

    public string GetSelectedConversionType()
    {
        if (!conversionDropdown.interactable)
            return null;

        return conversionDropdown.options[conversionDropdown.value].text;
    }
}
