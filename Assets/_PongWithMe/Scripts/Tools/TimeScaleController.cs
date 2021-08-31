using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public static class TimeScaleController
    {
        private const float ANIMATING_TWEEN_DURATION = 0.5F;
        private const float TIMESCALE_TWEEN_DURATION = 2F;
        private const float ENDGAME_TIMESCALE_TWEEN_DURATION = 1F;

        public static Tween AnimatingTimeScale()
        {
            return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, ANIMATING_TWEEN_DURATION)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true);
        }

        public static Tween PlayTimeScale()
        {
            return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, TIMESCALE_TWEEN_DURATION)
                .SetEase(Ease.InQuad)
                .SetUpdate(true);
        }

        public static Tween EndGameTimeScale()
        {
            return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, ENDGAME_TIMESCALE_TWEEN_DURATION)
                .SetEase(Ease.InQuad)
                .SetUpdate(true)
                .OnComplete(() => { StateManager.Instance.SetState(State.ShowScore); });
        }
    }
}