using UnityEngine;
using UnityEngine.UI;
using UniverseLib.Localization;
using System;

#if UNITY_2017_1_OR_NEWER
[RequireComponent(typeof(Text))]
#endif
public class LocalizedText : MonoBehaviour
{
    public string LocalizationKey;
    private Text _textComponent;

    private void Awake()
    {
        _textComponent = GetComponent<Text>();
    }

    private void OnEnable()
    {
        LocalizationManager.OnLanguageChanged += UpdateText;
        UpdateText(); // Update text immediately when the component is enabled
    }

    private void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= UpdateText;
    }

    private void UpdateText()
    {
        if (_textComponent && !string.IsNullOrEmpty(LocalizationKey))
        {
            _textComponent.text = LocalizationManager.GetString(LocalizationKey);
        }
    }
} 