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
    }
}