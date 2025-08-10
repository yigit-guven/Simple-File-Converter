using UnityEngine;

public class URLOpener : MonoBehaviour
{
    [Tooltip("Enter the URL you want to open.")]
    public string url = "https://";

    public void OpenURL()
    {
        if (!string.IsNullOrEmpty(url))
        {
            Application.OpenURL(url);
        }
        else
        {
            Debug.LogWarning("URL is empty or null!");
        }
    }
}
