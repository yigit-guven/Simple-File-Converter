using UnityEngine;
using System.IO;

public class OutputPathManager : MonoBehaviour
{
    public static bool useSameAsInput = true; // Always true now

    private const string SameAsInputKey = "UseSameAsInput";

    private void Awake()
    {
        LoadSettings();
    }

    // No longer needed since we always use same as input
    // public void SelectOutputFolder() removed

    // Kept for consistency (though it will always do the same thing)
    public void SetSameAsInput()
    {
        useSameAsInput = true;
        SaveSettings();
        Debug.Log("Output set to: Same as input file");
    }

    public static string GetFinalOutputPath(string inputFilePath)
    {
        if (!string.IsNullOrEmpty(inputFilePath))
        {
            return Path.GetDirectoryName(inputFilePath);
        }

        // Fallback if no input path is provided
        return Application.persistentDataPath;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt(SameAsInputKey, useSameAsInput ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        useSameAsInput = PlayerPrefs.GetInt(SameAsInputKey, 1) == 1; // Default to true
    }
}