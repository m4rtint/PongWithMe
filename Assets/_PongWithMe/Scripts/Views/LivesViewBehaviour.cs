using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class LivesViewBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _topLivesPosition = null;
        [SerializeField] private TMP_Text _rightLivesPosition = null;
        [SerializeField] private TMP_Text _bottomLivesPosition = null;
        [SerializeField] private TMP_Text _leftLivesPosition = null;

        private LivesViewModel _viewModel = null;
        
        public void Initialize(PlayerLives lives, List<IPaddle> players)
        {
            _viewModel = new LivesViewModel(lives, players);
            _viewModel.OnScoreUpdate += HandleOnScoreUpdate;
            SetupStyle();
        }

        private void SetupStyle()
        {
            
        }

        private void HandleOnScoreUpdate(Direction direction, int score)
        {
            switch (direction)
            {
                case Direction.Top:
                    _topLivesPosition.SetText(score.ToString());
                    break;
                case Direction.Bottom:
                    _bottomLivesPosition.SetText(score.ToString());
                    break;
                case Direction.Left:
                    _leftLivesPosition.SetText(score.ToString());
                    break;
                case Direction.Right:
                    _rightLivesPosition.SetText(score.ToString());
                    break;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    return;
            }
        }

        private void OnDestroy()
        {
            _viewModel.OnScoreUpdate -= HandleOnScoreUpdate;
        }
    }

    public class LivesViewModel
    {
        private PlayerLives _playersLives = null;
        private List<IPaddle> _players = null;

        public event Action<Direction, int> OnScoreUpdate;

        public LivesViewModel(PlayerLives lives, List<IPaddle> players)
        {
            _players = players;
            _playersLives = lives;
            _playersLives.OnBrickBreak += HandleOnBrickBreak;
        }

        private void HandleOnBrickBreak(int brickOwnerNumber, int numberOfLives)
        {
            var paddle = GetPlayerPaddle(brickOwnerNumber);
            OnScoreUpdate?.Invoke(paddle.PaddleDirection, numberOfLives);
        }
        
        private IPaddle GetPlayerPaddle(int brickOwnerNumber) 
        {
            foreach (var player in _players)
            {
                if (player.PlayerNumber == brickOwnerNumber)
                {
                    return player;
                }
            }

            return null;
        }
    }
}

