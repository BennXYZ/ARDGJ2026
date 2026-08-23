using DG.Tweening;
using UnityEngine;

public class BrainWobble : MonoBehaviour
{
    [SerializeField] private Vector3 distance;
    [SerializeField] private float time;

    void Awake()
    {
        transform.DOMove(distance, time).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
