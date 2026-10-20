using UnityEngine;

public class Comments : MonoBehaviour //https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
{
    [TextArea(10,15)] //https://docs.unity3d.com/ScriptReference/TextAreaAttribute.html
    public string Comment = "";
}
