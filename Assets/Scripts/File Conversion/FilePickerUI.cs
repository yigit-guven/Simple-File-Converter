using UnityEngine;
using TMPro;
using System.IO;

public class FilePickerUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text fileNameText;        // TMP text to show filename.type
    public GameObject panel1;            // First panel to show
    public GameObject panel2;            // Second panel to show

    public void PickFile()
    {
        if (NativeFilePicker.IsFilePickerBusy())
            return;

        // Allowed extensions (without the dot)
        string[] allowedExtensions = new string[]
        {
            "txt", "pdf", "doc", "docx", "xls", "xlsx"
        };

        NativeFilePicker.PickFile((path) =>
        {
            if (path == null)
            {
                Debug.Log("File picking cancelled.");
            }
            else
            {
                Debug.Log("Picked file: " + path);

                // Get filename with extension
                string fileName = Path.GetFileName(path);

                // Update TMP text
                fileNameText.text = fileName;

                // Show panels
                panel1.SetActive(true);
                panel2.SetActive(true);
            }
        }, allowedExtensions);
    }
}
