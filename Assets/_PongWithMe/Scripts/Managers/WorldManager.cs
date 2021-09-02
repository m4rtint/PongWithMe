using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        private string emily = "Play with emily";
        
        [SerializeField] private BricksBehaviour _bricksBehaviour = null;
        [SerializeField] private BallBehaviour _ballBehaviour = null;
        [SerializeField] private PlayersManager _playersManager = null;
        [SerializeField] private GoalsManager _goalsManager = null;
        [SerializeField] private MutatorBehaviour _mutatorBehaviour = null;

        [Title("Mutators")] 
        [SerializeField] private SplattersBehaviour _splattersBehaviour = null;
        
        [Title("User Interface")]
        [SerializeField] private LivesViewBehaviour _livesView = null;
        [SerializeField] private GameOverViewBehaviour _gameOverView = null;
        [SerializeField] private ScorePanelViewBehaviour _scorePanelView = null;

        private PlayerLives _playerLives = null;
        private Board _board = null;
        private IStateManager _stateManager = null;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            _stateManager = StateManager.Instance;
            _stateManager.OnStateChanged += HandleStateChanges;
        }

        private void Start()
        {
            new ColorPalette();
            
            //Board
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _board = new Board(bricks);
            _playerLives = new PlayerLives(_board, AMOUNT_OF_PLAYERS);
            _bricksBehaviour.Initialize(_board);
            
            // Ball
            _ballBehaviour.Initialize();
            
            // Players
            _playersManager.Initialize(_ballBehaviour, _playerLives);

            // Goals
            _goalsManager.Initialize(_playerLives);
            
            // Mutator
            _splattersBehaviour.Initialize();
            
            var mutatorManager = new MutatorManager(
                _playersManager.Players, 
                _playersManager, 
                _goalsManager,
                _playerLives,
                _ballBehaviour,
                _board, 
                _splattersBehaviour.Splatters);
            _mutatorBehaviour.Initialize(mutatorManager);

            // Interface
            _livesView.Initialize(_playerLives, _playersManager.Players);
            _gameOverView.Initialize(_playersManager);
            _scorePanelView.Initialize(_playersManager.Players, _playersManager, lives: 3);
            
            _stateManager.SetState(State.PreGame);
        }

        private void CleanUp()
        {
            _board.CleanUp();
            _playerLives.CleanUp();
            _bricksBehaviour.CleanUp();
            _ballBehaviour.CleanUp();
            _playersManager.CleanUp();
            
            _splattersBehaviour.CleanUp();

            _mutatorBehaviour.CleanUp();
        }

        private void Reset()
        {
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _board.Reset(bricks);
            _playerLives.Reset();
            _bricksBehaviour.Reset();
            _ballBehaviour.Reset();
            _playersManager.Reset();
            
            _goalsManager.Reset(_playersManager.Players);
            _splattersBehaviour.Reset();
            _mutatorBehaviour.Reset();
        }

        private void OnDestroy()
        {
            _stateManager.OnStateChanged -= HandleStateChanges;
        }

        private void HandleStateChanges(State state)
        {
            switch (state)
            {
                case State.PreGame:
                    TimeScaleController.PlayTimeScale();
                    CleanUp();
                    Reset();
                    break;
                case State.StartGame:
                    _stateManager.SetState(State.Play);
                    break;
                case State.Play:
                    TimeScaleController.PlayTimeScale();
                    break;
                case State.Animating:
                    break;
                case State.EndGame:
                    TimeScaleController.EndGameTimeScale();
                    break;
                case State.ShowScore:
                case State.GameOver:
                    break;
                default:
                    PanicHelper.Panic(new Exception("Should not have hit this game state"));
                    break;
            }   
        }
    }
}

