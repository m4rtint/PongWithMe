using DG.Tweening;
using UnityEngine;

namespace PongWithMe
{
    public static class TimeScaleController
    {
        private const float TIMESCALE_TWEEN_DURATION = 2F;
        private const float ENDGAME_TIMESCALE_TWEEN_DURATION = 1F;

        public static void PauseTimeScale()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, TIMESCALE_TWEEN_DURATION)
                .SetEase(Ease.OutQuint)
                .SetUpdate(true);
        }

        public static void PlayTimeScale()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, TIMESCALE_TWEEN_DURATION)
                .SetEase(Ease.InQuad)
                .SetUpdate(true);
        }

        public static void EndGameTimeScale()
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0, ENDGAME_TIMESCALE_TWEEN_DURATION)
                .SetEase(Ease.InQuad)
                .SetUpdate(true)
                .OnComplete(() => { StateManager.Instance.SetState(State.GameOver); });
        }
    }
}