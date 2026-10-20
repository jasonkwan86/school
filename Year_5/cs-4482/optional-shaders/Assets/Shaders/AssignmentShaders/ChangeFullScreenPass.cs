using UnityEngine;

[ExecuteInEditMode]
public class ControlFullScreenPass : MonoBehaviour
{
    [SerializeField] private FullScreenPassRendererFeature fullScreenPassFeature;
    [SerializeField] private bool enableFullScreenPass;
    
    void Start()
    {
        if (fullScreenPassFeature != null)
        {
            fullScreenPassFeature.SetActive(enableFullScreenPass); 
        }
    }
}