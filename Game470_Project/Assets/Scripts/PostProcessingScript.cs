using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingScript : MonoBehaviour
{
    public Volume PPV;

    [HideInInspector]
    public Vignette _vignette;

    private void Start()
    {
        PPV = GetComponent<Volume>();
        PPV.profile.TryGet<Vignette>(out _vignette);
    }

    public void LensView(bool spiritForm)
    {
        if (spiritForm)
        {
            _vignette.color.value = new Color(0, 201, 255);
            _vignette.intensity.value = 0.05f;
        }
        else
        {
            _vignette.color.value = new Color(0, 0, 0);
            _vignette.intensity.value = 0.383f;
        }
    }

}
