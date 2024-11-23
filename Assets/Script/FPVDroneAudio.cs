using System.Collections;
using UnityEngine;

public class FPVDroneAudio : MonoBehaviour
{
    public AudioSource loop, turnOn, turnOff, windHeavy;
    private FPVDrone drone;

    // settings
    public AnimationCurve volumeCurve;
    public AnimationCurve pitchCurve;
    public float volumeChangeSpeed;
    public float pitchChangeSpeed;
    public float startUpVolumeSpeed;
    public float startUpPitchSpeed;
    public float stopVolumeSpeed;
    public float stopPitchSpeed;
    public float maxWindSpeed;
    public float windVolumeSpeed;

    private bool starting;
    private bool stopping;

    void Start()
    {
        drone = GetComponent<FPVDrone>();
    }

    void Update()
    {
        if(drone.started)
        {
            if (starting)
            {
                loop.volume = Mathf.Lerp(loop.volume, volumeCurve.Evaluate(0), Time.deltaTime * startUpVolumeSpeed);
                loop.pitch = Mathf.Lerp(loop.pitch, pitchCurve.Evaluate(0), Time.deltaTime * startUpPitchSpeed);

                if (loop.volume >= volumeCurve.Evaluate(0) - 0.05F && loop.pitch >= pitchCurve.Evaluate(0) - 0.05F)
                    starting = false;
            }
            else
            {
                loop.volume = Mathf.Lerp(loop.volume, volumeCurve.Evaluate(Mathf.Abs(Input.GetAxis("Throttle"))), Time.deltaTime * volumeChangeSpeed);
                loop.pitch = Mathf.Lerp(loop.pitch, pitchCurve.Evaluate(Mathf.Abs(Input.GetAxis("Throttle"))), Time.deltaTime * pitchChangeSpeed);
            }
        }
        
        if(stopping)
        {
            loop.volume = Mathf.Lerp(loop.volume, 0, Time.deltaTime * stopVolumeSpeed);
            loop.pitch = Mathf.Lerp(loop.pitch, 0, Time.deltaTime * stopPitchSpeed);

            if (loop.volume <= 0.02F && loop.pitch <= 0.02F)
            {
                loop.volume = loop.pitch = 0;
                loop.Stop();
                stopping = false;
            }
        }

        windHeavy.volume = Mathf.Lerp(0, Mathf.Min(drone.mps / maxWindSpeed, 1), Time.deltaTime * windVolumeSpeed);
    }

    public void StartDrone()
    {
        starting = true;
        stopping = false;

        loop.volume = 0;
        loop.pitch = 0;
        loop.Play();
        turnOn.Play();
    }

    public void StopDrone()
    {
        stopping = true;
        starting = false;
        turnOff.Play();
    }
}
