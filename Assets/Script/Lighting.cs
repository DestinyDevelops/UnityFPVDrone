using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Lighting : MonoBehaviour
{
    [Header("Components")]
    public Light sun;
    public Volume globalVolume;

    [Header("Settings")]
    public float timeOfDay;
    public bool advanceTime;
    public float timeSpeed;
    public VolumetricClouds.CloudPresets cloudPreset;

    // hidden
    private VolumetricClouds volumetricClouds;

    void UpdateLighting()
    {
        if(volumetricClouds == null)
            globalVolume.profile.TryGet(out volumetricClouds);

        sun.transform.localRotation = Quaternion.Euler(timeOfDay * 3.6F, 0, 0);

        if (advanceTime)
            timeOfDay += Time.deltaTime * timeSpeed;
        if (timeOfDay >= 100)
            timeOfDay -= 100;
        if (timeOfDay < 0)
            timeOfDay = 100;

        volumetricClouds.cloudPreset = cloudPreset;
    }

    void Update()
    {
        UpdateLighting();
    }
}
