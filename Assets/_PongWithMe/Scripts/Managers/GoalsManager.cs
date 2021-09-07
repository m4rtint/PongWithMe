using System;
using System.Collections.Generic;
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
            _topGoal.Initialize();
            _leftGoal.Initialize();
            _rightGoal.Initialize();
            _bottomGoal.Initialize();
        }
        
        public void Set(List<IPaddle> players) 
        {
            foreach (var player in players)
            {
                Set(player);
            }
        }
        
        public void Set(IPaddle paddle)
        {
            var goal = GetGoal(paddle.PaddleDirection);
            goal.Set(paddle);
        }

        public void ActivateForceField(Direction direction, int forceFieldLives)
        {
            var goal = GetGoal(direction);
            goal.ActivateForceField(forceFieldLives);
        }

        public void Reset(List<IPaddle> players)
        {
            _topGoal.Reset();
            _leftGoal.Reset();
            _bottomGoal.Reset();
            _rightGoal.Reset();

            Set(players);
        }


        private GoalBehaviour GetGoal(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return _topGoal;
                case Direction.Right:
                    return _rightGoal;
                case Direction.Bottom:
                    return _bottomGoal;
                case Direction.Left:
                    return _leftGoal;
                default:
                    PanicHelper.Panic(new Exception("Paddles sent in through Goals Manager must have a direction."));
                    return null;
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
            _playerLives.ForceBrickBreakOwnedBy(paddle.PlayerNumber);
        }
        #endregion
    }
}