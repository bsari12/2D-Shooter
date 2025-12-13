using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem.LowLevel;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public CinemachineCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;

    void Awake()
    {
        noise =vCam.GetComponent<CinemachineBasicMultiChannelPerlin>() ;
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        StartCoroutine(ShakeRoutine(amplitude,frequency,duration));
    }

    IEnumerator ShakeRoutine(float amplitude, float frequency, float duration)
    {
        noise.AmplitudeGain =amplitude;
        noise.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
}
