using UnityEngine;

[RequireComponent(typeof(AudioSource)),DisallowMultipleComponent]
public class MuteSetter : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private MuteButton.MuteType muteType;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (MuteButton.IsMuted(muteType))
        {
            audioSource.volume = 0;
        }

        MuteButton.onMute += OnMute;
    }

    private void OnMute(MuteButton.MuteType triggeredMuteType, bool muted)
    {
        if (triggeredMuteType != muteType)
            return;

        audioSource.volume = muted ? 0 : 1;
    }

    private void OnDestroy()
    {
        MuteButton.onMute -= OnMute;
    }
}
