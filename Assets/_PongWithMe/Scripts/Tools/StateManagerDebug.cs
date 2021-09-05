using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class StateManagerDebug : MonoBehaviour
    {
        [Button]
        public void SetPlayState()
        {
            StateManager.Instance.SetState(State.Play);
        }

        [Button]
        public void SetAnimatingState()
        {
            StateManager.Instance.SetState(State.Animating);
            TimeScaleController.AnimatingTimeScale();
        }

        [Button]
        public void SetEndGameState()
        {
            StateManager.Instance.SetState(State.EndRound);
        }

        [Button]
        public void ShowEndGameScore()
        {
            StateManager.Instance.SetState(State.ShowScore);
        }
    }
}
