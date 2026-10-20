using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string translationKey;
    
    TextMeshProUGUI _textBox;

    void OnEnable()
    {
        _textBox = GetComponent<TextMeshProUGUI>();
        RefreshText();
    }

    public void RefreshText()
    {
        _textBox.text = LocalizationManager.Instance.GetTranslation(translationKey);
    }
}
