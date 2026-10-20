# Intro

Tools 1 assignment made with Unity 2022.3.62f1 using [Renaissance Coders' Editor Scripting tutorial.](https://www.youtube.com/playlist?list=PL4CCSwmU04MiCnps1DRmwIEEH7gP9X3qq)

Users can go through a wizard to design, create and save enemies in the form of scriptable objects and prefabs with their chosen data.

# Use

- Click on "Window/Enemy Designer"
- Select damage and weapon type in enum fields
- Drag "Cube" Prefab object into the prefab field, add a name and color, and click submit
- The corresponding scriptable object will be created in a class folder in `assets/resources/characterData/data` and the prefab in one of the folders in `assets/characters`

# Modification:

- Added custom font, icons
- Added color field
- Added color field requirement + helpbox warning when the color chosen isn't the color black.
- Added mesh renderer requirement + helpbox warning when the prefab doesn't have a `MeshRenderer` component
- Prefab created will be of the chosen color
