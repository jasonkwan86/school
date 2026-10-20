using UnityEngine;
using UnityEngine.UI;

public class ui_HeartController : MonoBehaviour
{
    [SerializeField] private Image[] uiHeartImages;
    
    public void UpdateHeartsUI(HealthComponent healthComponent)
    {
        for (int i = 0; i < uiHeartImages.Length; i++)
        {
            uiHeartImages[i].enabled = healthComponent.Health > i;
        }
    }
}
