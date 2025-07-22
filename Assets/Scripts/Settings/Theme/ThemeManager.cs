using UnityEngine;
using System;
using System.Collections.Generic;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager Instance;

    public List<ThemeData> themes;
    public int CurrentThemeIndex { get; private set; }

    public static event Action<ThemeData> OnThemeChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CurrentThemeIndex = PlayerPrefs.GetInt("themeIndex", 0);
            ApplyTheme(CurrentThemeIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTheme(int index)
    {
        if (index < 0 || index >= themes.Count) return;

        CurrentThemeIndex = index;
        PlayerPrefs.SetInt("themeIndex", index);
        PlayerPrefs.Save();

        ApplyTheme(index);
    }

    private void ApplyTheme(int index)
    {
        if (OnThemeChanged != null)
        {
            OnThemeChanged.Invoke(themes[index]);
        }
    }
}
