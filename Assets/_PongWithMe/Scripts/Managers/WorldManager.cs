using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class WorldManager : MonoBehaviour
    {
        private const int AMOUNT_OF_PLAYERS = 4;
        
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
        private IStateManager _stateManager = null;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            _stateManager = StateManager.Instance;
            _stateManager.SetState(State.PreGame);
        }

        private void Start()
        {
            new ColorPalette();
            _stateManager.OnStateChanged += HandleStateChanges;
            
            //Board
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _playerLives = new PlayerLives(bricks, AMOUNT_OF_PLAYERS);
            _bricksBehaviour.Initialize(bricks);
            
            // Ball
            _ballBehaviour.Initialize();
            
            // Players
            _playersManager.Initialize(_ballBehaviour, _playerLives);

            // Goals
            _goalsManager.Initialize(_playerLives);
            _goalsManager.Set(_playersManager.Players);
            
            // Mutator
            _splattersBehaviour.Initialize();
            
            var mutatorManager = new MutatorManager(
                _playersManager.Players, 
                _playersManager, 
                _goalsManager,
                _playerLives,
                _ballBehaviour,
                bricks, 
                _splattersBehaviour.Splatters);
            _mutatorBehaviour.Initialize(mutatorManager);

            // Interface
            _livesView.Initialize(_playerLives, _playersManager.Players);
            _gameOverView.Initialize(_playersManager);
            _scorePanelView.Initialize(_playersManager.Players, _playersManager, lives: 3);
        }

        private void CleanUp()
        {
            _playerLives.CleanUp();
            _bricksBehaviour.CleanUp();
            _ballBehaviour.CleanUp();
        }

        private void Reset()
        {
            var bricks = BoardFactory.Build(AMOUNT_OF_PLAYERS);
            _playerLives.Reset(bricks);
            _bricksBehaviour.Reset(bricks);
            _ballBehaviour.Reset();
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

