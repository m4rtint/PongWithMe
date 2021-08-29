using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class LivesViewBehaviour : MonoBehaviour
    {
        private const string PLAYER_LIVES_TEXT = "{0} Lives";
        private const float ANIMATION_DURATION = 0.5f;
        
        [SerializeField] private TMP_Text _topLivesPosition = null;
        [SerializeField] private TMP_Text _rightLivesPosition = null;
        [SerializeField] private TMP_Text _bottomLivesPosition = null;
        [SerializeField] private TMP_Text _leftLivesPosition = null;

        private LivesViewModel _viewModel = null;
        
        public void Initialize(PlayerLives lives, List<IPaddle> players)
        {
            _viewModel = new LivesViewModel(lives, players);
            _viewModel.OnScoreUpdate += HandleIndividualLiveUpdate;
            _viewModel.OnPlayerLivesUpdated += HandleOnPlayerLivesUpdated;
            SetupStyle(players);
            SetupScore(players);
        }

        private void SetupStyle(List<IPaddle> players)
        {
            foreach (var paddle in players)
            {
                SetupStyleFor(paddle);
            }
        }

        private void SetupStyleFor(IPaddle player)
        {
            var label = GetLivesTextLabel(player.PaddleDirection);
            label.color = player.PlayerColor;
        }

        private void SetupScore(List<IPaddle> players)
        {
            foreach (var player in players)
            {    
                HandleIndividualLiveUpdate(player.PaddleDirection, _viewModel.NumberOfLivesFor(player.PlayerNumber));
            }
        }

        private Tween MinimizePlayerScoreAnimation(TMP_Text label)
        {
            return label.transform.DOScale(Vector3.zero, ANIMATION_DURATION).SetEase(Ease.InBack).SetUpdate(true);
        }

        private Tween MaximizePlayerScoreAnimation(TMP_Text label)
        {
            return label.transform
                .DOScale(Vector3.one, ANIMATION_DURATION)
                .SetUpdate(true)
                .SetEase(Ease.OutBack);
        }
        
        #region Delegates
        private void HandleOnPlayerLivesUpdated(List<IPaddle> players)
        {
            foreach (var player in players)
            {
                var label = GetLivesTextLabel(player.PaddleDirection);
                var sequence = DOTween.Sequence();
                sequence.Append(MinimizePlayerScoreAnimation(label)).SetUpdate(true);
                sequence.AppendCallback(() =>
                {
                    SetupStyleFor(player);
                    HandleIndividualLiveUpdate(player.PaddleDirection, _viewModel.NumberOfLivesFor(player.PlayerNumber));
                });
                sequence.Append(MaximizePlayerScoreAnimation(label));
            }
        }
        
        private void HandleIndividualLiveUpdate(Direction direction, int score)
        {
            var label = GetLivesTextLabel(direction);
            SetLivesText(label, score);
        }
        
        #endregion

        private TMP_Text GetLivesTextLabel(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return _topLivesPosition;
                case Direction.Bottom:
                    return _bottomLivesPosition;
                case Direction.Left:
                    return _leftLivesPosition;
                case Direction.Right:
                    return _rightLivesPosition;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    return _topLivesPosition;
            }
        }

        private void SetLivesText(TMP_Text textView, int score)
        {
            textView.SetText(PLAYER_LIVES_TEXT, score);
        }

        private void OnDestroy()
        {
            if (_viewModel != null)
            {
                _viewModel.OnScoreUpdate -= HandleIndividualLiveUpdate;
            }
        }
    }
}

