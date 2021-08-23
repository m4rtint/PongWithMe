using UnityEngine;

namespace PongWithMe
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour _topPaddle = null;
        [SerializeField] private PaddleBehaviour _leftPaddle = null;
        [SerializeField] private PaddleBehaviour _bottomPaddle = null;
        [SerializeField] private PaddleBehaviour _rightPaddle = null;
        
        private IBall _ball = null;
        
        public void Initialize(IBall ball)
        {
            _ball = ball;
        }
        
        private void Start()
        {
            var input = new PongInput(0);                
            var player = new PlayerPaddle(input, 0, Direction.Top);
            _topPaddle.Initialize(player);
            
            var ai = new AIInput();
            var aiPaddle = new AIPaddle(ai, 1, Direction.Left, _ball);
            _leftPaddle.Initialize(aiPaddle);
            
            var ai2 = new AIInput();
            var aiPaddle2 = new AIPaddle(ai2, 2, Direction.Bottom, _ball);
            _bottomPaddle.Initialize(aiPaddle2);
            
            var ai3 = new AIInput();
            var aiPaddle3 = new AIPaddle(ai3, 3, Direction.Right, _ball);
            _rightPaddle.Initialize(aiPaddle3);
        }
    }
}

