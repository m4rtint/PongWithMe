using System;
using UnityEngine;

namespace PongWithMe
{
    public class GoalsManager : MonoBehaviour
    {
        [SerializeField] private GoalBehaviour _topGoal = null;
        [SerializeField] private GoalBehaviour _leftGoal = null;
        [SerializeField] private GoalBehaviour _bottomGoal = null;
        [SerializeField] private GoalBehaviour _rightGoal = null;

        private IPlayerLives _playerLives = null;
        
        public void Initialize(IPlayerLives playerLives)
        {
            _playerLives = playerLives;
        }
        
        public void Set(IPaddle paddle)
        {
            switch (paddle.PaddleDirection)
            {
                case Direction.Top:
                    _topGoal.Set(paddle);
                    break;
                case Direction.Bottom:
                    _bottomGoal.Set(paddle);
                    break;
                case Direction.Left:
                    _leftGoal.Set(paddle);
                    break;
                case Direction.Right:
                    _rightGoal.Set(paddle);
                    break;
                default:
                    PanicHelper.Panic(new Exception("Paddles sent in through Goals Manager must have a direction."));
                    break;
            }
        }

        #region Mono
        private void Awake()
        {
            _topGoal.OnGoalHit += HandleOnGoalHit;
            _bottomGoal.OnGoalHit += HandleOnGoalHit;
            _leftGoal.OnGoalHit += HandleOnGoalHit;
            _rightGoal.OnGoalHit += HandleOnGoalHit;
        }

        private void OnDestroy()
        {
            _topGoal.OnGoalHit -= HandleOnGoalHit;
            _bottomGoal.OnGoalHit -= HandleOnGoalHit;
            _leftGoal.OnGoalHit -= HandleOnGoalHit;
            _rightGoal.OnGoalHit -= HandleOnGoalHit;
        }
        
        #endregion
        
        
        #region Delegate
        private void HandleOnGoalHit(IPaddle paddle)
        {
            _playerLives.BreakBrickOwnedBy(paddle.PlayerNumber);
        }
        #endregion
    }
}