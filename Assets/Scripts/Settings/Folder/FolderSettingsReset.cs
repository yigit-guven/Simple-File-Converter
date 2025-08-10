using UnityEngine;

public class PlayerPrefsResetter : MonoBehaviour
{
    /// <summary>
    /// Call this method to reset both PlayerPrefs to false.
    /// </summary>
    public void ResetPlayerPrefsToDefault()
    {
        PlayerPrefs.SetString("replaceonconversion", "false");
        PlayerPrefs.SetString("autodownload", "false");
        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs 'replaceonconversion' and 'autodownload' reset to false.");
    }
}
