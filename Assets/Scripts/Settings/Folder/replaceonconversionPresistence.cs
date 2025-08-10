using UnityEngine;
using UnityEngine.UI;

public class ImagePersistenceController : MonoBehaviour
{
    [Tooltip("Assign the Image GameObject to control its persistence")]
    public GameObject imageObject;

    void Start()
    {
        bool isPersistent = false; // default false

        if (PlayerPrefs.HasKey("replaceonconversion"))
        {
            string val = PlayerPrefs.GetString("replaceonconversion").ToLower();
            isPersistent = (val == "1" || val == "true");
        }
        else
        {
            Debug.Log("PlayerPref 'replaceonconversion' not found. Using default: false.");
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
