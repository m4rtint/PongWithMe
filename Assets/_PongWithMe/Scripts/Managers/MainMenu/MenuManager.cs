using Sirenix.OdinInspector;
using UnityEngine;

namespace PongWithMe
{
    public class MenuManager : MonoBehaviour
    {
        [Title("Components")]
        [SerializeField] private NetworkLauncher _networkLauncher = null;
        
        [Title("Views")]
        [SerializeField] private MatchMakingViewBehaviour _matchMakingView = null;
        [SerializeField] private StartScreenViewBehaviour _startScreenView = null;
        
        private void Start()
        {
            _networkLauncher.Initialize();
            _matchMakingView.Initialize(_networkLauncher);
            _startScreenView.Initialize(() =>
            {
                _matchMakingView.ShowView();
            });
        }
    } 
}

