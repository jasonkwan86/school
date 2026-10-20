using System;
using Types;
using UnityEditor;
using UnityEngine;

public class EnemyDesignerWindow : EditorWindow
{
    private Texture2D headerSectionTexture;
    private Texture2D mageSectionTexture;
    private Texture2D warriorSectionTexture;
    private Texture2D rogueSectionTexture;

    private Texture2D mageTexture;
    private Texture2D warriorTexture;
    private Texture2D rogueTexture;

    private static float maxColor = 255f;
    private Color headerSectionColor = new Color(13f / maxColor, 32f / maxColor, 44f / maxColor, 1f);

    private Rect headerSection;
    private Rect mageSection;
    private Rect warriorSection;
    private Rect rogueSection;
    
    private Rect mageIconSection;
    private Rect warriorIconSection;
    private Rect rogueIconSection;

    private GUISkin skin;
    
    private static MageData mageData;
    private static WarriorData warriorData;
    private static RogueData rogueData;

    private float iconSize = 80;
    private float iconOffset = 8;
    
    public static MageData MageInfo
    {
        get { return mageData; }
    }
    public static WarriorData WarriorInfo
    {
        get { return warriorData; }
    }
    public static RogueData RogueInfo
    {
        get { return rogueData; }
    }
    
    [MenuItem("Window/Enemy Designer")]
    static void OpenWindow()
    {
        EnemyDesignerWindow window = (EnemyDesignerWindow)GetWindow(typeof(EnemyDesignerWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
        InitData();
        skin = Resources.Load<GUISkin>("guiStyles/EnemyDesignerSkin");
    }

    public static void InitData()
    {
        mageData = (MageData)CreateInstance(typeof(MageData));
        warriorData = (WarriorData)CreateInstance(typeof(WarriorData));
        rogueData = (RogueData)CreateInstance(typeof(RogueData));
    }
    
    void InitTextures()
    {
        headerSectionTexture = new Texture2D(1, 1);
        headerSectionTexture.SetPixel(0, 0, headerSectionColor);
        headerSectionTexture.Apply();
        
        mageSectionTexture = Resources.Load<Texture2D>("icons/editor_mage_gradient");
        warriorSectionTexture = Resources.Load<Texture2D>("icons/editor_warrior_gradient");
        rogueSectionTexture = Resources.Load<Texture2D>("icons/editor_rogue_gradient");

        mageTexture = Resources.Load<Texture2D>("icons/mage_icon");
        warriorTexture = Resources.Load<Texture2D>("icons/warrior_icon");
        rogueTexture = Resources.Load<Texture2D>("icons/rogue_icon");
    }

    private void OnGUI()
    {
        DrawLayouts();
        DrawHeader();
        DrawMageSettings();
        DrawWarriorSettings();
        DrawRogueSettings();
    }

    void DrawLayouts()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 50;
        
        mageSection.x = 0;
        mageSection.y = 50;
        mageSection.width = Screen.width / 3f;
        mageSection.height = Screen.width - 50;

        mageIconSection.x = mageSection.x + mageSection.width / 2f - iconSize / 2;
        mageIconSection.y = mageSection.y + iconOffset;
        mageIconSection.width = iconSize;
        mageIconSection.height = iconSize;
        
        warriorSection.x = Screen.width / 3f;
        warriorSection.y = 50;
        warriorSection.width = Screen.width / 3f;
        warriorSection.height = Screen.width - 50;
        
        warriorIconSection.x = warriorSection.x + warriorSection.width / 2f - iconSize / 2;
        warriorIconSection.y = warriorSection.y + iconOffset;
        warriorIconSection.width = iconSize;
        warriorIconSection.height = iconSize;
        
        rogueSection.x = Screen.width / 3f * 2;
        rogueSection.y = 50;
        rogueSection.width = Screen.width / 3f;
        rogueSection.height = Screen.width - 50;
        
        rogueIconSection.x = rogueSection.x + rogueSection.width / 2f - iconSize / 2;
        rogueIconSection.y = rogueSection.y + iconOffset;
        rogueIconSection.width = iconSize;
        rogueIconSection.height = iconSize;
        
        GUI.DrawTexture(headerSection, headerSectionTexture);
        GUI.DrawTexture(mageSection, mageSectionTexture);
        GUI.DrawTexture(warriorSection, warriorSectionTexture);
        GUI.DrawTexture(rogueSection, rogueSectionTexture);
        GUI.DrawTexture(mageIconSection, mageTexture);
        GUI.DrawTexture(warriorIconSection, warriorTexture);
        GUI.DrawTexture(rogueIconSection, rogueTexture);
    }

    void DrawHeader()
    {
        GUILayout.BeginArea(headerSection);
        
        GUILayout.Label("Enemy Designer", skin.GetStyle("Header1"));
        
        GUILayout.EndArea();
    }

    void DrawMageSettings()
    {
        GUILayout.BeginArea(mageSection);
        
        GUILayout.Space(iconSize + iconOffset);
        
        GUILayout.Label("Mage", skin.GetStyle("ClassHeader"));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Damage", skin.GetStyle("FieldHeader"));
        mageData.damageType = (MageDamageType)EditorGUILayout.EnumPopup(mageData.damageType);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Weapon", skin.GetStyle("FieldHeader"));
        mageData.weaponType = (MageWeaponType)EditorGUILayout.EnumPopup(mageData.weaponType);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.MAGE);
        }
        
        GUILayout.EndArea();
    }

    void DrawWarriorSettings()
    {
        GUILayout.BeginArea(warriorSection);
        
        GUILayout.Space(iconSize + iconOffset);
        
        GUILayout.Label("Warrior", skin.GetStyle("ClassHeader"));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Class", skin.GetStyle("FieldHeader"));
        warriorData.classType = (WarriorClassType)EditorGUILayout.EnumPopup(warriorData.classType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weapon", skin.GetStyle("FieldHeader"));
        warriorData.weaponType = (WarriorWeaponType)EditorGUILayout.EnumPopup(warriorData.weaponType);
        GUILayout.EndHorizontal();
        
        
        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.WARRIOR);
        }
        
        GUILayout.EndArea();
    }

    void DrawRogueSettings()
    {
        GUILayout.BeginArea(rogueSection);
        
        GUILayout.Space(iconSize + iconOffset);
        
        GUILayout.Label("Rogue", skin.GetStyle("ClassHeader"));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Strategy", skin.GetStyle("FieldHeader"));
        rogueData.strategyType = (RogueStrategyType)EditorGUILayout.EnumPopup(rogueData.strategyType);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Weapon", skin.GetStyle("FieldHeader"));
        rogueData.weaponType = (RogueWeaponType)EditorGUILayout.EnumPopup(rogueData.weaponType);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Create!", GUILayout.Height(40)))
        {
            GeneralSettings.OpenWindow(GeneralSettings.SettingsType.ROGUE);
        }
        
        GUILayout.EndArea();
    }
}

public class GeneralSettings : EditorWindow
{
    public enum SettingsType
    {
        MAGE,
        WARRIOR,
        ROGUE,
    }

    private static SettingsType dataSetting;
    private static GeneralSettings window;

    public static void OpenWindow(SettingsType settingType)
    {
        dataSetting = settingType;
        window = (GeneralSettings)EditorWindow.GetWindow(typeof(GeneralSettings));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    void OnGUI()
    {
        switch (dataSetting)
        {
            case SettingsType.MAGE:
                DrawingSettings((CharacterData)EnemyDesignerWindow.MageInfo);
                break;
            case SettingsType.WARRIOR:
                DrawingSettings((CharacterData)EnemyDesignerWindow.WarriorInfo);
                break;
            case SettingsType.ROGUE:
                DrawingSettings((CharacterData)EnemyDesignerWindow.RogueInfo);
                break;
        }
    }

    void DrawingSettings(CharacterData charData)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefab");
        charData.prefab = (GameObject)EditorGUILayout.ObjectField(charData.prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Health");
        charData.maxHealth = EditorGUILayout.FloatField(charData.maxEnergy);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Max Energy");
        charData.maxEnergy = EditorGUILayout.FloatField(charData.maxEnergy);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Power");
        charData.power = EditorGUILayout.Slider(charData.power, 0, 100);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("% Crit Chance");
        charData.critChance = EditorGUILayout.Slider(charData.critChance, 0, charData.power);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Name");
        charData.name = EditorGUILayout.TextField(charData.name);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Color");
        charData.color = EditorGUILayout.ColorField(charData.color);
        EditorGUILayout.EndHorizontal();

        if (charData.prefab == null)
        {
            EditorGUILayout.HelpBox("This enemy needs a [Prefab] before it can be created.", MessageType.Error);
        }
        else if (charData.prefab.GetComponent<MeshRenderer>() == null)
        {
            EditorGUILayout.HelpBox("This [Prefab] needs a mesh renderer component before it can be created.", MessageType.Warning);
        }
        else if (String.IsNullOrEmpty(charData.name))
        {
            EditorGUILayout.HelpBox("This enemy needs a [Name] before it can be created.", MessageType.Warning);
        }
        else if (charData.color.r == 0 && charData.color.g == 0 && charData.color.b == 0)
        {
            EditorGUILayout.HelpBox("Choose a color that isn't the default black color!", MessageType.Warning);
        }
        else if (GUILayout.Button("Finish and Save", GUILayout.Height(40)))
        {
            SaveCharacterData();
            window.Close();
        }
    }

    void SaveCharacterData()
    {
        string prefabPath;
        string newPrefabPath = "Assets/prefabs/characters/";
        string dataPath = "Assets/resources/characterData/data/";

        switch (dataSetting)
        {
            case SettingsType.MAGE:
                dataPath += "mage/" + EnemyDesignerWindow.MageInfo.name + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignerWindow.MageInfo, dataPath);
                
                newPrefabPath += "mage/" + EnemyDesignerWindow.MageInfo.name + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.MageInfo.prefab);
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject magePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                if (!magePrefab.GetComponent<Mage>())
                {
                    magePrefab.AddComponent(typeof(Mage));
                }
                magePrefab.GetComponent<Mage>().mageData = EnemyDesignerWindow.MageInfo;
                magePrefab.GetComponent<MeshRenderer>().sharedMaterial.color = EnemyDesignerWindow.MageInfo.color;
                break;
            case SettingsType.WARRIOR:
                dataPath += "warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignerWindow.WarriorInfo, dataPath);
                
                newPrefabPath += "warrior/" + EnemyDesignerWindow.WarriorInfo.name + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.WarriorInfo.prefab);
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject warriorPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                if (!warriorPrefab.GetComponent<Warrior>())
                {
                    warriorPrefab.AddComponent(typeof(Warrior));
                }
                warriorPrefab.GetComponent<Warrior>().warriorData = EnemyDesignerWindow.WarriorInfo;
                warriorPrefab.GetComponent<MeshRenderer>().sharedMaterial.color = EnemyDesignerWindow.WarriorInfo.color;
                break;
            case SettingsType.ROGUE:
                dataPath += "rogue/" + EnemyDesignerWindow.RogueInfo.name + ".asset";
                AssetDatabase.CreateAsset(EnemyDesignerWindow.RogueInfo, dataPath);
                
                newPrefabPath += "rogue/" + EnemyDesignerWindow.RogueInfo.name + ".prefab";
                prefabPath = AssetDatabase.GetAssetPath(EnemyDesignerWindow.RogueInfo.prefab);
                AssetDatabase.CopyAsset(prefabPath, newPrefabPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                GameObject roguePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(newPrefabPath, typeof(GameObject));
                if (!roguePrefab.GetComponent<Rogue>())
                {
                    roguePrefab.AddComponent(typeof(Rogue));
                }
                roguePrefab.GetComponent<Rogue>().rogueData = EnemyDesignerWindow.RogueInfo;
                roguePrefab.GetComponent<MeshRenderer>().sharedMaterial.color = EnemyDesignerWindow.RogueInfo.color;
                break;
        }
    }
}