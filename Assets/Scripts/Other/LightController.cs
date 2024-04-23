using System.Threading.Tasks;
using UnityEngine;
public class LightController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private float fadeTime;
    private float initialIntensity;
    private async void Awake()
    {
        initialIntensity = sun.intensity;
        await FadeIn();
    }
    public async Task  FadeIn()
    {
        sun.intensity = initialIntensity - 0.8f;
        float currentIntensity = sun.intensity;

        float t = 0;

        while(t < 1)
        {
            sun.intensity = Mathf.Lerp(currentIntensity, initialIntensity, t);
            t += Time.deltaTime / fadeTime;
            await Task.Yield();
        }
        sun.intensity = initialIntensity;
    }
    public async Task FadeOut()
    {
        float currentIntensity = initialIntensity;
        float targetIntensity = sun.intensity = initialIntensity - 0.6f;

        float t = 0;

        while (t < 1)
        {
            sun.intensity = Mathf.Lerp(currentIntensity, targetIntensity, t);
            t += Time.deltaTime / fadeTime;
            await Task.Yield();
        }
        sun.intensity = targetIntensity;
    }
}
