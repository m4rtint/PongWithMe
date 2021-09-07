using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PongWithMe
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        // TODO - THIS IS PLACE HOLDER, NETWORKCORE SHOULD BE PASSED INTO THE BUTTON ITSELF.
        [SerializeField] private Button _leaveButton = null;

        public void Initialize()
        {
            
        }
        
        #region PUN
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
            
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }
        
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }
        #endregion

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading With Number Of Players: {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }
        
        #region Mono
        private void Awake()
        {
            _leaveButton.onClick.AddListener(LeaveRoom);
        }
        
        #endregion

    }
}