using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Button)), DisallowMultipleComponent]
public class ButtonSoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioMixerGroup mixerGroup;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button button = GetComponent<Button>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
        audioSource.Stop();
        button.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        audioSource.Stop();
        audioSource.Play();
    }
}
