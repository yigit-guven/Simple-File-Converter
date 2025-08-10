using UnityEngine;
using UnityEngine.UI;

public class ImagePersistenceController2 : MonoBehaviour
{
    [Tooltip("Assign the Image GameObject to control its persistence")]
    public GameObject imageObject;

    void Start()
    {
        bool isPersistent = false; // default false

        if (PlayerPrefs.HasKey("autodownload"))
        {
            string val = PlayerPrefs.GetString("autodownload").ToLower();
            isPersistent = (val == "1" || val == "true");
        }
        else
        {
            Debug.Log("PlayerPref 'autodownload' not found. Using default: false.");
        }

        if (isPersistent)
        {
            DontDestroyOnLoad(imageObject);
            Debug.Log("Image set to persistent.");
        }
        else
        {
            Debug.Log("Image NOT set to persistent.");
        }
    }
}
