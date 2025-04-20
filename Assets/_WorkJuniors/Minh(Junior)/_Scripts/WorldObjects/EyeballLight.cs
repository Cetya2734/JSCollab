using UnityEngine;

[RequireComponent(typeof(Light))]
public class EyeballLight : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 0.1f;
    public bool useSmoothFlicker = false;

    private Light eyeLight;
    private float targetIntensity;
    private float timer;

    void Start()
    {
        eyeLight = GetComponent<Light>();
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= flickerSpeed)
        {
            timer = 0f;
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        if (useSmoothFlicker)
        {
            eyeLight.intensity = Mathf.Lerp(eyeLight.intensity, targetIntensity, Time.deltaTime * 10f);
        }
        else
        {
            eyeLight.intensity = targetIntensity;
        }
    }
}
