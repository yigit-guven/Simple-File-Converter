using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Text;

public class ConvertButtonHandler : MonoBehaviour
{
    [SerializeField] private FilePickerController filePicker;
    [SerializeField] private ConversionSelector conversionSelector;
    [SerializeField] private Button convertButton;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        convertButton.gameObject.SetActive(false);
        conversionSelector.OnConversionTypeSelected.AddListener(OnConversionTypeSelected);
    }

    private void OnConversionTypeSelected(bool isValid)
    {
        convertButton.gameObject.SetActive(isValid);
        convertButton.interactable = isValid;
    }

    public void OnConvertButtonPressed()
    {
        string inputPath = filePicker.GetSelectedFilePath();
        if (string.IsNullOrEmpty(inputPath))
        {
            UpdateStatus("No input file selected!", true);
            return;
        }

        string outputExt = conversionSelector.GetSelectedConversionType();
        if (string.IsNullOrEmpty(outputExt))
        {
            UpdateStatus("No output format selected!", true);
            return;
        }

        if (!outputExt.StartsWith("."))
            outputExt = "." + outputExt;

        string outputDirectory = OutputPathManager.GetFinalOutputPath(inputPath);
        if (string.IsNullOrEmpty(outputDirectory))
        {
            UpdateStatus("Output directory is not set!", true);
            return;
        }

        try
        {
            // Create directory if it doesn't exist
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputFileName = Path.GetFileNameWithoutExtension(inputPath) + outputExt;
            string outputPath = Path.Combine(outputDirectory, outputFileName);

            // Handle file name conflicts
            outputPath = GetUniqueFileName(outputPath);

            UpdateStatus($"Converting {Path.GetFileName(inputPath)} to {outputExt}...");

            bool success = ConvertFile(inputPath, outputPath, outputExt);

            if (success)
            {
                UpdateStatus($"Conversion successful!\nSaved to: {outputPath}");
            }
            else
            {
                UpdateStatus("Conversion failed!", true);
            }
        }
        catch (System.Exception e)
        {
            UpdateStatus($"Error: {e.Message}", true);
        }
    }

    private bool ConvertFile(string inputPath, string outputPath, string outputExt)
    {
        string inputExt = Path.GetExtension(inputPath).ToLower();
        string inputContent = File.ReadAllText(inputPath);

        try
        {
            switch (inputExt)
            {
                case ".txt":
                    return ConvertFromTxt(inputContent, outputPath, outputExt);
                case ".csv":
                    return ConvertFromCsv(inputContent, outputPath, outputExt);
                case ".json":
                    return ConvertFromJson(inputContent, outputPath, outputExt);
                case ".xml":
                    return ConvertFromXml(inputContent, outputPath, outputExt);
                case ".pdf":
                    return ConvertFromPdf(inputContent, outputPath, outputExt);
                default:
                    return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private bool ConvertFromTxt(string content, string outputPath, string outputExt)
    {
        if (outputExt == ".pdf")
        {
            // Simple PDF creation (would need proper PDF library for real implementation)
            File.WriteAllText(outputPath, $"PDF Content (simulated):\n\n{content}");
            return true;
        }
        else if (outputExt == ".csv")
        {
            // Convert text lines to simple CSV
            var lines = content.Split('\n');
            StringBuilder csv = new StringBuilder();
            foreach (var line in lines)
            {
                csv.AppendLine($"\"{line.Trim()}\"");
            }
            File.WriteAllText(outputPath, csv.ToString());
            return true;
        }
        return false;
    }

    private bool ConvertFromCsv(string content, string outputPath, string outputExt)
    {
        if (outputExt == ".json")
        {
            // Simple CSV to JSON conversion
            var lines = content.Split('\n');
            StringBuilder json = new StringBuilder("[\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var fields = lines[i].Split(',');
                json.Append("  {");
                for (int j = 0; j < fields.Length; j++)
                {
                    json.Append($"\"field{j}\":{fields[j].Trim()}");
                    if (j < fields.Length - 1) json.Append(", ");
                }
                json.Append("}");
                if (i < lines.Length - 1) json.Append(",");
                json.AppendLine();
            }
            json.Append("]");
            File.WriteAllText(outputPath, json.ToString());
            return true;
        }
        else if (outputExt == ".txt")
        {
            // Simple CSV to text conversion
            var lines = content.Split('\n');
            StringBuilder text = new StringBuilder();
            foreach (var line in lines)
            {
                text.AppendLine(line.Replace(",", " | "));
            }
            File.WriteAllText(outputPath, text.ToString());
            return true;
        }
        return false;
    }

    private bool ConvertFromJson(string content, string outputPath, string outputExt)
    {
        // Implement JSON conversions here
        return false;
    }

    private bool ConvertFromXml(string content, string outputPath, string outputExt)
    {
        // Implement XML conversions here
        return false;
    }

    private bool ConvertFromPdf(string content, string outputPath, string outputExt)
    {
        // Implement PDF conversions here
        return false;
    }

    private string GetUniqueFileName(string path)
    {
        if (!File.Exists(path)) return path;

        string directory = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string extension = Path.GetExtension(path);

        int counter = 1;
        string newPath;
        do
        {
            newPath = Path.Combine(directory, $"{fileName}_{counter}{extension}");
            counter++;
        } while (File.Exists(newPath));

        return newPath;
    }

    private void UpdateStatus(string message, bool isError = false)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = isError ? Color.red : Color.white;
        }
        Debug.Log(message);
    }
}