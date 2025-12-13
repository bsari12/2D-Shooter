using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1f)
    {
        StartCoroutine(PlaySFXCoroutine(audioClip,volume));
    }

    IEnumerator PlaySFXCoroutine(AudioClip audioClip, float volume = 1f)
    {
        AudioSource audiosource =gameObject.AddComponent<AudioSource>();
        audiosource.clip =audioClip;
        audiosource.volume = volume;
        audiosource.Play();

        yield return new WaitForSeconds(audioClip.length*2f);

        Destroy(audiosource);
    }
}
