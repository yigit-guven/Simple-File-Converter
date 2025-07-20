using UnityEngine;
using System.IO;
using NativeFilePickerNamespace; // UnityNativeFilePicker namespace

public class OutputPathManager : MonoBehaviour
{
    public static string outputPath = "";
    public static bool useSameAsInput = false;

    private const string OutputPathKey = "OutputPath";
    private const string SameAsInputKey = "UseSameAsInput";

    private void Awake()
    {
        LoadSettings();
    }

    // Open folder picker using UnityNativeFilePicker (only works on mobile)
    public void SelectOutputFolder()
    {
#if UNITY_ANDROID || UNITY_IOS
        NativeFilePicker.Permission permission = NativeFilePicker.PickFolder((path) =>
        {
            if (!string.IsNullOrEmpty(path))
            {
                outputPath = path;
                useSameAsInput = false;
                SaveSettings();
                Debug.Log("Output folder set to: " + outputPath);
            }
            else
            {
                Debug.Log("Folder picking cancelled or failed.");
            }
        });

        Debug.Log("Folder pick permission: " + permission);
#else
        Debug.LogWarning("SelectOutputFolder: Folder picking only supported on Android/iOS.");
#endif
    }

    // For platforms without folder picker, or to set to same as input
    public void SetSameAsInput()
    {
        useSameAsInput = true;
        outputPath = "";
        SaveSettings();
        Debug.Log("Output set to: Same as input file");
    }

    public static string GetFinalOutputPath(string inputFilePath)
    {
        if (useSameAsInput && !string.IsNullOrEmpty(inputFilePath))
        {
            return Path.GetDirectoryName(inputFilePath);
        }
        return outputPath;
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetString(OutputPathKey, outputPath);
        PlayerPrefs.SetInt(SameAsInputKey, useSameAsInput ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        outputPath = PlayerPrefs.GetString(OutputPathKey, "");
        useSameAsInput = PlayerPrefs.GetInt(SameAsInputKey, 0) == 1;
    }
}
