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
                var label = GetLivesTextLabel(paddle.PaddleDirection);
                label.color = paddle.PlayerColor;
            }
        }

        private void SetupScore(List<IPaddle> players)
        {
            foreach (var player in players)
            {
                HandleIndividualLiveUpdate(player.PaddleDirection, _viewModel.NumberOfLivesFor(player.PlayerNumber));
            }
        }


        private void SwapPlayerScoreAnimation(TMP_Text label)
        {
            var animationDuration = 1.0f;
            label.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    label.transform
                        .DOScale(Vector3.one, animationDuration)
                        .SetUpdate(true)
                        .SetEase(Ease.OutBack);
                });
        }
        
        #region Delegates
        private void HandleOnPlayerLivesUpdated(List<IPaddle> players)
        {
            SetupStyle(players);
            SetupScore(players);
            SwapPlayerScoreAnimation(_bottomLivesPosition);
            SwapPlayerScoreAnimation(_topLivesPosition);
            SwapPlayerScoreAnimation(_leftLivesPosition);
            SwapPlayerScoreAnimation(_rightLivesPosition);
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

