# Intro

Tools 2 assignment made with Unity 2022.3.62f1.

Users can go through a wizard (under `Localization Editor`) to create, edit and delete localization data in the form of scriptable objects.

# Use

## In Editor
- Click on "Window/Localization Editor"
- Manage languages
    - Click on the `+` sign on the first row to add a new language
    - Click on the `X` sign on the language's cell in the first row to delete it
- Manage language key IDs
    - Click on the `+` sign on the column to add a new language key identifier
    - Click on the `X` sign on the key's cell in the first column
- Fill in the intersection cells to add translation value for languages

## In Scene
- Add the `LocalizedTest` script on any `TextMeshProUGUI` GameObject and add the language key you want to use to its `Translation Key` field
- Ensure the `LocalizationManager` is present and has a `LanguageTranslation` game object assigned
