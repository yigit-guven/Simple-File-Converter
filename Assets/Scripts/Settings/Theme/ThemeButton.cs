using UnityEngine;

public class ThemeButton : MonoBehaviour
{
    public int themeIndex;

    public void SetThisTheme()
    {
        ThemeManager.Instance.SetTheme(themeIndex);
    }
}
