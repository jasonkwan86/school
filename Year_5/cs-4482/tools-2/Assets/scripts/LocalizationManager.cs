using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class LocalizationManager : MonoBehaviour
{
    [SerializeField] private MasterLocalization masterLocalization;
    [SerializeField] private List<LocalizedText> textToRefresh;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameObject nextMenu;
    [SerializeField] private GameObject currentMenu;

    private LanguageTranslation _selectedLanguageTranslation;
    
    public static LocalizationManager Instance { get; private set; }
    
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        List<TMP_Dropdown.OptionData> options = new ();
        
        foreach (LanguageTranslation languageTranslation in masterLocalization.languageTranslations)
        {
            options.Add(new TMP_Dropdown.OptionData(languageTranslation.GetLanguage()));
        }
        dropdown.options = options;
        _selectedLanguageTranslation = masterLocalization.languageTranslations.First();
    }
    
    public void AssignSelectedLanguage(int index)
    {
        string languageName = dropdown.options[dropdown.value].text;
        _selectedLanguageTranslation = masterLocalization.GetLanguageTranslation(languageName);
        foreach (LocalizedText text in textToRefresh)
        {
            text.RefreshText();
        }
    }

    public string GetTranslation(string translationKey)
    {
        return masterLocalization.GetLanguageTranslationValue(_selectedLanguageTranslation.GetLanguage(), translationKey);
    }

    public void ContinueToNextMenu()
    {
        ForwardMenu(true);
    }

    public void BackToLanguageSelectorMenu()
    {
        ForwardMenu(false);
    }

    private void ForwardMenu(bool forward)
    {
        nextMenu.SetActive(forward);
        currentMenu.SetActive(!forward);
    }

    public void HideCurrentMenu()
    {
        currentMenu.SetActive(false);
    }
}
