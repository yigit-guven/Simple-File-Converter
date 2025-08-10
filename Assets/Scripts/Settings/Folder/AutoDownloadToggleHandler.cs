using UnityEngine;
using UnityEngine.UI;

public class AutoDownloadToggleHandler : MonoBehaviour
{
    [Tooltip("Assign the Toggle UI component for autodownload")]
    public Toggle autoDownloadToggle;

    void Start()
    {
        // Initialize toggle state from PlayerPrefs
        bool isOn = PlayerPrefs.HasKey("autodownload") &&
                    (PlayerPrefs.GetString("autodownload").ToLower() == "1" ||
                     PlayerPrefs.GetString("autodownload").ToLower() == "true");
        autoDownloadToggle.isOn = isOn;

        // Add listener for changes
        autoDownloadToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetString("autodownload", isOn ? "true" : "false");
        PlayerPrefs.Save();
        Debug.Log($"PlayerPref 'autodownload' set to {isOn}");
    }

    void OnDestroy()
    {
        // Clean up listener
        if (autoDownloadToggle != null)
            autoDownloadToggle.onValueChanged.RemoveListener(OnToggleChanged);
    }
}
