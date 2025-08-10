using UnityEngine;
using UnityEngine.UI;

public class ReplaceOnConversionToggleHandler : MonoBehaviour
{
    [Tooltip("Assign the Toggle UI component for replaceonconversion")]
    public Toggle replaceonconversionToggle;

    void Start()
    {
        // Initialize toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.HasKey("replaceonconversion") &&
                    (PlayerPrefs.GetString("replaceonconversion").ToLower() == "1" ||
                     PlayerPrefs.GetString("replaceonconversion").ToLower() == "true");
        replaceonconversionToggle.isOn = isOn;

        // Add listener for changes
        replaceonconversionToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetString("replaceonconversion", isOn ? "true" : "false");
        PlayerPrefs.Save();
        Debug.Log($"PlayerPref 'replaceonconversion' set to {isOn}");
    }

    void OnDestroy()
    {
        // Clean up listener
        if (replaceonconversionToggle != null)
            replaceonconversionToggle.onValueChanged.RemoveListener(OnToggleChanged);
    }
}
