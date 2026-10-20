using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Master Localization", menuName = "Custom/Master Localization")]
public class MasterLocalization : ScriptableObject
{
    public static readonly string AssetPath = "Assets/resources/localizationData/data/";
    
    public List<string> translationKeys;
    public List<LanguageTranslation> languageTranslations;

    public LanguageTranslation GetLanguageTranslation(string languageName)
    {
        return languageTranslations.Find(language => language.GetLanguage() == languageName);
    }

    public void AddLanguage(string languageName)
    {
        string dataPath = AssetPath + languageName + ".asset";
        LanguageTranslation languageTranslation = CreateInstance<LanguageTranslation>();
        languageTranslation.name = languageName;
        languageTranslations.Add(languageTranslation);
        AssetDatabase.CreateAsset(languageTranslation, dataPath);
        EditorUtility.SetDirty(this); 
    }

    public string GetLanguageTranslationValue(string languageName, string key)
    {
        LanguageTranslation language = GetLanguageTranslation(languageName);
        if (language == null)
        {
            Debug.Log("Language is null");
        }
        Translation translation = language.GetTranslationWithKey(key);
        if (translation == null)
        {
            Debug.Log("No translation for key: " + key);
            return "";
        }
        return translation.translationValue;
    }

    public void ChangeLanguageTranslationValue(string languageName, string key, string newTranslationValue)
    {
        LanguageTranslation language = GetLanguageTranslation(languageName);
        if (language == null)
        {
            Debug.Log("Language is null");
        }
        language.ChangeTranslationForKey(key, newTranslationValue);
    }
    
    public void DeleteTranslationKey(string key)
    {
        translationKeys.Remove(key);
        foreach (LanguageTranslation languageTranslation in languageTranslations)
        {
            languageTranslation.DeleteKeyTranslation(key);
            EditorUtility.SetDirty(languageTranslation); 
        }
        EditorUtility.SetDirty(this); 
        // AssetDatabase.SaveAssets();
    }
    
    public void DeleteLanguage(LanguageTranslation languageTranslation)
    {
        languageTranslations.Remove(languageTranslation);
        string pathToDelete = AssetDatabase.GetAssetPath(languageTranslation);
        if (!string.IsNullOrEmpty(pathToDelete))
        {
            AssetDatabase.DeleteAsset(pathToDelete);
        }
        EditorUtility.SetDirty(this); 
    }
}
