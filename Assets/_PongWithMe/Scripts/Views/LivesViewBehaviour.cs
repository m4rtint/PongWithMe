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
            SetupStyle(players);
            SetupScore(players);
        }

        private void SetupStyle(List<IPaddle> players)
        {
            foreach (var paddle in players)
            {
                switch (paddle.PaddleDirection)
                {
                    case Direction.Top:
                        _topLivesPosition.color = paddle.PlayerColor;
                        break;
                    case Direction.Bottom:
                        _bottomLivesPosition.color = paddle.PlayerColor;
                        break;
                    case Direction.Left:
                        _leftLivesPosition.color = paddle.PlayerColor;
                        break;
                    case Direction.Right:
                        _rightLivesPosition.color = paddle.PlayerColor;
                        break;
                    default:
                        PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                        return;
                }
            }
        }

        private void SetupScore(List<IPaddle> players)
        {
            foreach (var player in players)
            {
                HandleOnScoreUpdate(player.PaddleDirection, _viewModel.NumberOfLivesFor(player.PlayerNumber));
            }
        }
        
        private void HandleOnScoreUpdate(Direction direction, int score)
        {
            switch (direction)
            {
                case Direction.Top:
                    SetLivesText(_topLivesPosition, score);
                    break;
                case Direction.Bottom:
                    SetLivesText(_bottomLivesPosition, score);
                    break;
                case Direction.Left:
                    SetLivesText(_leftLivesPosition, score);
                    break;
                case Direction.Right:
                    SetLivesText(_rightLivesPosition, score);
                    break;
                default:
                    PanicHelper.Panic(new Exception("Paddle should always have a direction"));
                    return;
            }
        }

        private void SetLivesText(TMP_Text textView, int score)
        {
            textView.SetText("{0} Lives", score);
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

        public int NumberOfLivesFor(int owner)
        {
            return _playersLives.GetPlayerLives(owner);
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

