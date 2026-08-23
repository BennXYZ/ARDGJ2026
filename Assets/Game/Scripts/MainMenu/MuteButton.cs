using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MuteButton : MonoBehaviour
{
    public static event Action<MuteType, bool> onMute;
    private static Dictionary<MuteType, bool> muted;

    [SerializeField] private MuteType muteType;
    [SerializeField] private UnityEvent onMuted;
    [SerializeField] private UnityEvent onUnMuted;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Mute);
    }

    public static bool IsMuted(MuteType muteType)
    {
        muted ??= new Dictionary<MuteType, bool>();
        muted.TryAdd(muteType, false);
        return muted[muteType];
    }

    public void Mute()
    {
        muted ??= new Dictionary<MuteType, bool>();
        muted.TryAdd(muteType, false);
        muted[muteType] = !muted[muteType];
        onMute?.Invoke(muteType, muted[muteType]);
    }

    public enum MuteType
    {
        SFX,
        Music
    }
}
