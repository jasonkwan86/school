using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Ensure the name of the scriptable object is the name of the language
[CreateAssetMenu(fileName = "Default Translation", menuName = "Custom/Language Translation")]
public class LanguageTranslation : ScriptableObject
{
    public TranslationDictionary translationDictionary = new ();
    
    public void AddTranslation(Translation translation)
    {
        translationDictionary.translations.Add(translation);
        SaveToDisk();
    }

    public void ChangeTranslationForKey(string key, string newTranslationValue)
    {
        translationDictionary.ChangeTranslationForKey(key, newTranslationValue);
    }

    public void DeleteKeyTranslation(string key)
    {
        translationDictionary.DeleteTranslationKey(key);
        SaveToDisk();
    }

    public Translation GetTranslationWithKey(string key)
    {
        return translationDictionary.GetTranslation(key);
    }

    public string GetLanguage()
    {
        return name;
    }

    private void SaveToDisk()
    {
        EditorUtility.SetDirty(this); 
        AssetDatabase.SaveAssets();
    }
}

[Serializable]
public class TranslationDictionary
{
    public List<Translation> translations = new ();

    public Translation GetTranslation(string key)
    {
        return translations.Find(t => t.key == key);
    }

    public Translation GetTranslationAt(int index)
    {
        return translations[index];
    }

    public void ChangeTranslationForKey(string key, string newTranslationValue)
    {
        Translation translation = translations.Find(translation => translation.key == key);
        if (translation == null)
        {
            Debug.Log("Translation was not found with key: " + key);
        }
        translation.translationValue = newTranslationValue;
    }

    public void DeleteTranslationKey(string key)
    {
        Translation translationToDelete = translations.Find(translation => translation.key == key);
        translations.Remove(translationToDelete);
    }
    
    public int TranslationCount()
    {
        return translations.Count;
    }
}

[Serializable]
public class Translation
{
    public string key;
    public string translationValue;

    public Translation(string key, string translationValue)
    {
        this.key = key;
        this.translationValue = translationValue;
    }

    public override string ToString()
    {
        return key + ": " + translationValue;
    }
}