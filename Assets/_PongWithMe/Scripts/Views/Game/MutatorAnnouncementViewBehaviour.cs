using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    [RequireComponent(typeof(TMP_Text))]
    public class MutatorAnnouncementViewBehaviour : MonoBehaviour
    {
        private const float LABEL_ANIMATION_DURATION = 0.5F;
        private const float DELAY_HIDE_DURATION = 1F;
        private TMP_Text _announcementLabel = null;
        private MutatorManager _mutatorManager = null;

        public void Initialize(MutatorManager mutatorManager)
        {
            _mutatorManager = mutatorManager;
            _mutatorManager.OnMutatorPicked += HandleMutatorPicked;
        }

        public void CleanUp()
        {
            _announcementLabel.transform.localScale = Vector3.zero;
            SetLabel("");
        }
        
        private void HandleMutatorPicked(string announcement)
        {
            SetLabel(announcement);
            ShowAnnouncementAnimation();
        }

        private void SetLabel(string announcement)
        {
            _announcementLabel.text = announcement;
        }

        private void ShowAnnouncementAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(ShowLabel().SetEase(Ease.OutCubic)).SetUpdate(true);
            sequence.Append(HideLabel().SetDelay(DELAY_HIDE_DURATION).SetEase(Ease.OutQuint));
        }

        private Tween ShowLabel()
        {
            return _announcementLabel.transform.DOScale(Vector3.one, LABEL_ANIMATION_DURATION);
        }

        private Tween HideLabel()
        {
            return _announcementLabel.transform.DOScale(Vector3.zero, LABEL_ANIMATION_DURATION);
        }
        
        #region Mono
        private void Awake()
        {
            _announcementLabel = GetComponent<TMP_Text>();
        }

        private void OnDestroy()
        {
            if (_mutatorManager != null)
            {
                _mutatorManager.OnMutatorPicked -= HandleMutatorPicked;
            }
        }
        #endregion
    }
}
