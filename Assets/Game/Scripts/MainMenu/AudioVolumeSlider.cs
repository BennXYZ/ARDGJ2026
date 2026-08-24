using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    private AudioMixer audioMixer;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SliderValueChanged);
        audioMixer = audioMixerGroup.audioMixer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (audioMixer.GetFloat(audioMixerGroup.name, out float volume))
        {
            slider.value = -(volume / - 80 - 1);
        }

        SliderValueChanged(slider.value);
    }

    private void SliderValueChanged(float newValue)
    {
        audioMixer.SetFloat(audioMixerGroup.name, -80 * (1f - newValue));
    }
}
