using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class LightFlicker : MonoBehaviour
{
    public Light2D light2D;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float minFlickerDuration = 0.25f; 
    public float maxFlickerDuration = 1f;

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }

        StartFlicker();
    }

    void StartFlicker()
    {
        Flicker();
    }

    void Flicker()
    {
        float newIntensity = Random.Range(minIntensity, maxIntensity);
        float duration = Random.Range(minFlickerDuration, maxFlickerDuration);
        DOTween.To(() => light2D.intensity, x => light2D.intensity = x, newIntensity, duration).OnComplete(() => Flicker());
    }
}
