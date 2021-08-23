using UnityEngine;

namespace PongWithMe
{
    public class PlayersManager : MonoBehaviour
    {
        [SerializeField] private PaddleBehaviour[] _playerPaddles = null;
        // THIS SHOULD BE INJECTED IN
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        public void Initialize(BallBehaviour ball)
        {
            
        }
        
        private void Start()
        {
            var input = new PongInput(0);                
            var player = new PlayerPaddle(input, 0, Direction.Top);
            _playerPaddles[0].Initialize(player);
            
            var ai = new AIInput();
            var aiPaddle = new AIPaddle(ai, 1, Direction.Left, _ballBehaviour.gameObject);
            _playerPaddles[1].Initialize(aiPaddle);
            
            var ai2 = new AIInput();
            var aiPaddle2 = new AIPaddle(ai2, 1, Direction.Bottom, _ballBehaviour.gameObject);
            _playerPaddles[2].Initialize(aiPaddle2);
            
            var ai3 = new AIInput();
            var aiPaddle3 = new AIPaddle(ai3, 1, Direction.Right, _ballBehaviour.gameObject);
            _playerPaddles[3].Initialize(aiPaddle3);
        }
    }
}

