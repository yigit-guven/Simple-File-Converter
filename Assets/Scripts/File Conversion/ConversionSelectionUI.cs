using UnityEngine;
using TMPro;

public class ConversionSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Dropdown conversionDropdown; // dropdown with conversion options
    public TMP_Text selectedConversionText; // TMP to show selected conversion

    private string currentSelection = null; // session-only save (resets on scene/app close)

    private void Start()
    {
        // Ensure TMP text is hidden at start
        if (selectedConversionText != null)
            selectedConversionText.gameObject.SetActive(false);

        // Subscribe to dropdown event
        conversionDropdown.onValueChanged.AddListener(OnConversionSelected);
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        conversionDropdown.onValueChanged.RemoveListener(OnConversionSelected);
    }

    private void OnConversionSelected(int index)
    {
        if (conversionDropdown.options.Count == 0)
            return;

        // Save current choice for this session
        currentSelection = conversionDropdown.options[index].text;

        // Show it in TMP text
        if (selectedConversionText != null)
        {
            selectedConversionText.text = currentSelection;
            selectedConversionText.gameObject.SetActive(true);
        }
    }

    // Getter so other scripts can ask what was selected
    public string GetCurrentSelection()
    {
        return currentSelection;
    }
}
