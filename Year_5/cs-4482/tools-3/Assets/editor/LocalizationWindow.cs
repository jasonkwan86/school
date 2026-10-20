using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LocalizationWindow : EditorWindow
{
    private Texture2D _headerSectionTexture;
    private Rect _headerSection;
    private readonly int _cellWidth = 200;
    private readonly int _cellHeight = 50;

    private MasterLocalization _masterLocalization;
    private List<LanguageHeaderSection> _languageHeaderSections;
    private List<LanguageKeySection> _languageKeySections;
    private List<IntersectionDataSection> _intersectionDataSections;
    private List<string> _languages;
    
    private AddLanguageKeySection _addLanguageKeySection;
    private AddLanguageSection _addLanguageSection;
    private GUISkin _skin;
    
    private string _languageKeyToAdd = "";
    private string _languageToAdd = "";

    
    [MenuItem("Window/Localization Editor")]
    static void OpenWindow()
    {
        LocalizationWindow window = (LocalizationWindow)GetWindow(typeof(LocalizationWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        ReinitializeLanguages();
        InitTextures();
        _skin = Resources.Load<GUISkin>("guiStyles/LocalizationEditorSkin");
    }

    private void ReinitializeLanguages()
    {
        InitLanguageObjects();
        InitLanguages();
    }

    private void InitLanguageObjects()
    {
        _masterLocalization = Resources.Load<MasterLocalization>("localizationData/data/Master Localization");

        // Ensure each key exists in the languages
        foreach (LanguageTranslation languageTranslation in _masterLocalization.languageTranslations)
        {
            HashSet<string> existingTranslationKeys = new HashSet<string>(languageTranslation.translationDictionary.translations.Select(
                translation => translation.key
            ));

            foreach (string translationKey in _masterLocalization.translationKeys)
            {
                bool hasKey = existingTranslationKeys.Contains(translationKey);
                if (!hasKey)
                {
                    // Add a new Translation object to the TranslationDictionary
                    languageTranslation.AddTranslation(new Translation(translationKey, ""));
                }
            }
        }
    }

    private void InitLanguages()
    {
        _languages = new List<string>(_masterLocalization.languageTranslations.Select(languageTranslation => languageTranslation.GetLanguage()));
        IEnumerable<string> translationKeys = _masterLocalization.translationKeys;
        _languageHeaderSections = new List<LanguageHeaderSection>();
        _languageKeySections = new List<LanguageKeySection>();
        _intersectionDataSections = new List<IntersectionDataSection>();
        for (int i = 0; i < _languages.Count; i++)
        {
            _languageHeaderSections.Add(
                new LanguageHeaderSection(
                    _masterLocalization.languageTranslations[i],
                    new Vector2(i * _cellWidth + _cellWidth, 0),
                    new Vector2(_cellWidth, _cellHeight)
                )
            );
        }
        
        for (int i = 0; i < translationKeys.Count(); i++)
        {
            _languageKeySections.Add(
                new LanguageKeySection(
                    translationKeys.ElementAt(i),
                    GetAllTranslationsWithKey(_masterLocalization, translationKeys.ElementAt(i)),
                    new Vector2(0, (i + 1) * _cellHeight),
                    new Vector2(_cellWidth, _cellHeight)
                )
            );
        }

        _addLanguageKeySection = new AddLanguageKeySection(
            new Vector2(0, (translationKeys.Count() + 1) * _cellHeight),
            new Vector2(_cellWidth, _cellHeight * 3)
            );

        _addLanguageSection = new AddLanguageSection(
            new Vector2((_languages.Count + 1) * _cellWidth, 0),
            new Vector2(_cellWidth, _cellHeight * 3)
        );

        for (int languageIndex = 0; languageIndex < _masterLocalization.languageTranslations.Count; languageIndex++)
        {
            for (int keyIndex = 0; keyIndex < _masterLocalization.languageTranslations[languageIndex].translationDictionary.TranslationCount(); keyIndex++)
            {
                _intersectionDataSections.Add(
                    new IntersectionDataSection(
                        _masterLocalization.languageTranslations[languageIndex],
                        _masterLocalization.languageTranslations[languageIndex].translationDictionary.GetTranslationAt(keyIndex),
                        new Vector2((languageIndex + 1) * _cellWidth, (keyIndex + 1) * _cellHeight),
                        new  Vector2(_cellWidth, _cellHeight)
                    )
                );
            }
        }
    }

    private void InitTextures()
    {
        _headerSectionTexture = new Texture2D(1, 1);
        _headerSectionTexture.SetPixel(0, 0, Color.clear);
        _headerSectionTexture.Apply();
    }
    
    private void OnGUI()
    {
        DrawHeader();
        DrawLanguageHeaderSections();
        DrawKeyHeaderSections();
        DrawIntersectionDataSections();
        DrawAddSections();
    }

    private void DrawHeader()
    {
        _headerSection.x = 0;
        _headerSection.y = 0;
        _headerSection.width = _cellWidth;
        _headerSection.height = _cellHeight;
        
        GUILayout.BeginArea(_headerSection);
        GUILayout.Label("Translation Key", _skin.GetStyle("TranslationKeyHeader"));
        GUILayout.EndArea();
        
        GUI.DrawTexture(_headerSection, _headerSectionTexture);
    }

    private void DrawLanguageHeaderSections()
    {
        foreach (LanguageHeaderSection languageHeaderSection in _languageHeaderSections)
        {
            GUILayout.BeginArea(languageHeaderSection.SectionRect);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _masterLocalization.DeleteLanguage(languageHeaderSection.LanguageTranslation);
                ReinitializeLanguages();
            }
            else
            {
                GUILayout.Label(languageHeaderSection.LanguageTranslation.GetLanguage(), _skin.GetStyle("LanguageHeader"));
            }
            GUILayout.EndArea();
            GUI.DrawTexture(languageHeaderSection.SectionRect, languageHeaderSection.SectionTexture);
        }
    }

    private void DrawKeyHeaderSections()
    {
        foreach (LanguageKeySection languageKeySection in _languageKeySections)
        {
            GUILayout.BeginArea(languageKeySection.SectionRect);
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                _masterLocalization.DeleteTranslationKey(languageKeySection.CurrentKey);
                ReinitializeLanguages();
            }
            else
            {
                string newKey = GUILayout.TextArea(languageKeySection.CurrentKey);
                if (newKey != languageKeySection.CurrentKey)
                {
                    languageKeySection.ChangeCurrentKey(_masterLocalization, newKey);
                    ReinitializeLanguages();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUI.DrawTexture(languageKeySection.SectionRect, languageKeySection.SectionTexture);
        }
    }

    private void DrawIntersectionDataSections()
    {
        foreach (IntersectionDataSection intersectionDataSection in _intersectionDataSections)
        {
            GUILayout.BeginArea(intersectionDataSection.SectionRect);
            string changedText = GUILayout.TextArea(intersectionDataSection.Translation.translationValue);
            bool textWasChanged = changedText != intersectionDataSection.Translation.translationValue;
            intersectionDataSection.Translation.translationValue = changedText;
            if (textWasChanged)
            {
                _masterLocalization.ChangeLanguageTranslationValue(
                    intersectionDataSection.LanguageTranslation.name,
                    intersectionDataSection.Translation.key,
                    changedText
                );
                SaveToDisk(intersectionDataSection.LanguageTranslation);
            }
            GUILayout.EndArea();
            GUI.DrawTexture(intersectionDataSection.SectionRect, intersectionDataSection.SectionTexture);
        }
    }

    private void DrawAddSections()
    {
        DrawAddLanguageKeySection();
        DrawAddLanguageSection();
    }

    private void DrawAddLanguageKeySection()
    {
        GUILayout.BeginArea(_addLanguageKeySection.SectionRect);
        GUILayout.Label("Add key here");
        _languageKeyToAdd = GUILayout.TextArea(_languageKeyToAdd);
        bool keyAlreadyExists = _masterLocalization.translationKeys.Contains(_languageKeyToAdd);
        if (keyAlreadyExists)
        {
            EditorGUILayout.HelpBox("This key already exists. Choose a different key name.", MessageType.Warning);
        }
        GUI.enabled = _languageKeyToAdd.Trim().Length != 0 && !keyAlreadyExists;
        if (GUILayout.Button("Submit!", GUILayout.Height(40)) && !keyAlreadyExists)
        {
            _masterLocalization.translationKeys.Add(_languageKeyToAdd);
            ReinitializeLanguages();
            SaveToDisk(_masterLocalization);
            
            _languageKeyToAdd = "";
        }
        GUILayout.EndArea();
        GUI.DrawTexture(_addLanguageKeySection.SectionRect, _addLanguageKeySection.SectionTexture);
        GUI.enabled = true;
    }

    private void DrawAddLanguageSection()
    {
        GUILayout.BeginArea(_addLanguageSection.SectionRect);
        GUILayout.Label("Add language here");
        _languageToAdd = GUILayout.TextArea(_languageToAdd);
        bool languageAlreadyExists = _languages.Contains(_languageToAdd);
        if (languageAlreadyExists)
        {
            EditorGUILayout.HelpBox("This language already exists. Choose a different name.", MessageType.Error);
        }
        GUI.enabled = _languageToAdd.Trim().Length != 0 && !languageAlreadyExists;
        if (GUILayout.Button("Submit!", GUILayout.Height(40)) && !languageAlreadyExists)
        {
            AddLanguage(_languageToAdd);
            ReinitializeLanguages();
            _languageToAdd = "";
        }
        GUILayout.EndArea();
        GUI.DrawTexture(_addLanguageSection.SectionRect, _addLanguageSection.SectionTexture);
        GUI.enabled = true;
    }

    private void AddLanguage(string languageName)
    {
        _masterLocalization.AddLanguage(languageName);
    }

    private static IEnumerable<Translation> GetAllTranslationsWithKey(MasterLocalization masterLocalization, string key)
    { 
        return masterLocalization.languageTranslations.Select(translation => translation.GetTranslationWithKey(key));
    }

    private void SaveToDisk(ScriptableObject so)
    {
        EditorUtility.SetDirty(so); 
        // AssetDatabase.SaveAssets();
    }
}

public class LanguageHeaderSection
{
    public LanguageTranslation LanguageTranslation;
    public Texture2D SectionTexture;
    public Rect SectionRect;

    public LanguageHeaderSection(LanguageTranslation languageTranslation, Vector2 position, Vector2 size)
    {
        LanguageTranslation = languageTranslation;
        SectionTexture = new Texture2D(1, 1);
        SectionTexture.SetPixel(0, 0, Color.clear);
        SectionTexture.Apply();
        SectionRect = new Rect(position, size);
    }
}

public class LanguageKeySection
{
    // List of all translations with this key
    public string CurrentKey;
    private IEnumerable<Translation> translations;
    public Texture2D SectionTexture;
    public Rect SectionRect;

    public LanguageKeySection(string currentKey, IEnumerable<Translation> translations, Vector2 position, Vector2 size)
    {
        CurrentKey = currentKey;
        this.translations = translations;
        SectionTexture = new Texture2D(1, 1);
        SectionTexture.SetPixel(0, 0, Color.clear);
        SectionTexture.Apply();
        SectionRect = new Rect(position, size);
    }

    public void ChangeCurrentKey(MasterLocalization masterLocalization, string newKey)
    {
        int indexToReplace = masterLocalization.translationKeys.FindIndex(key => key == CurrentKey);
        masterLocalization.translationKeys[indexToReplace] = newKey;

        foreach (Translation translation in translations)
        {
            if (translation == null)
            {
                Debug.Log("Why translation null :(");
                continue;
            }
            translation.key = newKey;
        }
        
        CurrentKey = newKey;
        SaveToDisk(masterLocalization);
    }

    private void SaveToDisk(ScriptableObject so)
    {
        EditorUtility.SetDirty(so); 
        AssetDatabase.SaveAssets();
    }
}

public class IntersectionDataSection
{
    public Texture2D SectionTexture;
    public Rect SectionRect;

    public LanguageTranslation LanguageTranslation;
    public Translation Translation;

    public IntersectionDataSection(LanguageTranslation languageTranslation, Translation translation, Vector2 position, Vector2 size)
    {
        LanguageTranslation = languageTranslation;
        Translation = translation;
        SectionTexture = new Texture2D(1, 1);
        SectionTexture.SetPixel(0, 0, Color.clear);
        SectionTexture.Apply();
        SectionRect = new Rect(position, size);
    }
}

public class AddLanguageKeySection
{
    public Texture2D SectionTexture;
    public Rect SectionRect;

    public AddLanguageKeySection(Vector2 position, Vector2 size)
    {
        SectionTexture = new Texture2D(1, 1);
        SectionTexture.SetPixel(0, 0, Color.clear);
        SectionTexture.Apply();
        SectionRect = new Rect(position, size);
    }
}

public class AddLanguageSection
{
    public Texture2D SectionTexture;
    public Rect SectionRect;

    public AddLanguageSection(Vector2 position, Vector2 size)
    {
        SectionTexture = new Texture2D(1, 1);
        SectionTexture.SetPixel(0, 0, Color.clear);
        SectionTexture.Apply();
        SectionRect = new Rect(position, size);
    }
}