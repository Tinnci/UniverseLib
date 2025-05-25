using System.Collections.Generic; // For Dictionary or other collections
using System.IO; // For File operations
using Newtonsoft.Json; // For JSON deserialization
using System;

namespace UniverseLib.Localization
{
    public class LocalizationManager
    {
        // Singleton instance
        private static LocalizationManager _instance;
        public static LocalizationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocalizationManager();
                    // Optional: Initialize localization here (e.g., load default language)
                    _instance.LoadLanguage("en"); // Load default language
                }
                return _instance;
            }
        }

        private Dictionary<string, string> _localizedStrings = new Dictionary<string, string>();
        private string _currentLanguage = "en"; // Default language
        public static event Action OnLanguageChanged; // Add language changed event

        // Private constructor to prevent external instantiation
        private LocalizationManager() { }

        // Method to load language resources (placeholder for now)
        public void LoadLanguage(string languageCode)
        {
            _currentLanguage = languageCode;
            _localizedStrings.Clear();

            // 实际加载逻辑
            try
            {
                // 假设语言文件与程序集在同一目录下的 "Languages" 文件夹中
                string languageFileName = $"{languageCode}.json";
                string languagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Languages");
                string filePath = Path.Combine(languagesFolderPath, languageFileName); // Modify to use two-parameter overload
                if (File.Exists(filePath))
                {
                    string jsonContent = File.ReadAllText(filePath);
                    _localizedStrings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);
                    Universe.Log($"Loaded language: {languageCode}"); // Log successful loading
                }
                else
                {
                    Universe.LogWarning($"Language file not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Universe.LogError($"Failed to load language '{languageCode}': {ex.Message}");
            }

            // Trigger UI updates for currently visible elements
            OnLanguageChanged?.Invoke();
        }

        // Method to get localized string by key
        public static string GetString(string key, string defaultValue = null)
        {
            // Use the singleton instance
            if (Instance._localizedStrings.TryGetValue(key, out string localizedString))
            {
                return localizedString;
            }

            // If key not found
            Universe.LogWarning($"Missing localization key: '{key}' for language '{Instance._currentLanguage}'");

            // TODO: Try to get from fallback language if implemented

            return defaultValue ?? $"MissingKey:{key}";
        }

        // Method to change the current language
        public void ChangeLanguage(string newLanguageCode)
        {
            LoadLanguage(newLanguageCode);
        }

        // TODO: Add methods for changing language and triggering UI updates
    }
} 