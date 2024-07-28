using UnityEngine;

namespace Infrastructure.Settings
{
    public static class PlayerPrefsExtended
    {
        public static string GetString(string key, string defaultValue = null) => PlayerPrefs.GetString(key, defaultValue);

        public static float GetFloat(string key, float defaultValue = 0) => PlayerPrefs.GetFloat(key, defaultValue);

        public static int GetInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

        public static bool GetBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        public static bool HasKey(string key) => PlayerPrefs.HasKey(key);

        public static void SetString(string key, string value) => PlayerPrefs.SetString(key, value);

        public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

        public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

        public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

        public static void Save() => PlayerPrefs.Save();
    }
}