# Intro

Tools 3 assignment made with Unity 2022.3.62f1, based off of Tools 2, with code taken from the lecture examples on dialogue trees.

Users can go through a wizard (under `Localization Editor`) to create, edit and delete localization data in the form of scriptable objects.
Users can setup dialogue trees with [xNode](https://github.com/Siccity/xNode) to choose to respond with different dialogue choices.

# Use

- Go into scene named `Tools-3` and click play.
- Select a language from the dropdown menu and enter the dialogue

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
