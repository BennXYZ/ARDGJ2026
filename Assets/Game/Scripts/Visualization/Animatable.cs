using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class Animatable : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private AnimationEvent[] currentEvents;
    private Action onEndEvent;
    private float previousNormalizedProgress = 0;

    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (currentEvents != null)
        {
            foreach (AnimationEvent actionEvent in currentEvents)
            {
                actionEvent.Update(Time.deltaTime);
            }

            var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0);
            if (previousNormalizedProgress > currentAnimatorState.normalizedTime % 1f)
            {
                onEndEvent?.Invoke();
                if (currentEvents != null)
                {
                    foreach (AnimationEvent actionEvent in currentEvents)
                    {
                        actionEvent.Reset();
                    }
                }
            }
            previousNormalizedProgress = currentAnimatorState.normalizedTime % 1f;
        }
    }

    public void PlayAnimation(string state, int delay = 0, Action onEnd = null) => PlayAnimation(state, delay, onEnd, Array.Empty<AnimationEvent>());

    public void PlayAnimation(string state, int delay = 0, Action onEnd = null, params AnimationEvent[] events)
    {
        previousNormalizedProgress = 0;
        StopAllCoroutines();
        if (delay > 0)
        {
            StartCoroutine(DelayAnimation(state, delay, onEnd, events));
            return;
        }

        StartCoroutine(DoPlayAnimation(state, onEnd, events));
    }

    private IEnumerator DelayAnimation(string state, int delay, Action onEnd, AnimationEvent[] events)
    {
        yield return new WaitForSeconds(delay / 10f);
        yield return DoPlayAnimation(state, onEnd, events);
    }

    private IEnumerator DoPlayAnimation(string state, Action onEnd, AnimationEvent[] events)
    {
        animator.Play(state, 0, 0f);
        currentEvents = null;
        onEndEvent = null;
        previousNormalizedProgress = 0;
        yield return null; // animators don't update their "current state" immediately. Wait 1 Frame to ensure we get the correct clip length;
        foreach(AnimationEvent actionEvent in events)
        {
            actionEvent.callTime -= 1;
        }
        onEndEvent = onEnd;
        currentEvents = events;
    }

    public class AnimationEvent
    {
        public Action action;
        public int callTime;
        private bool triggered;
        private float timer;

        public AnimationEvent(Action action, int callTime)
        {
            this.action = action;
            this.callTime = callTime;
        }

        public void Reset()
        {
            triggered = false;
            timer = 0;
        }

        public void Update(float deltaTime)
        {
            if (triggered)
                return;
            timer += deltaTime;
            if (timer * 10 > callTime)
            {
                action?.Invoke();
                triggered = true;
            }
        }
    }
}
