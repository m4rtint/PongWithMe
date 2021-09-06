using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PongWithMe
{
    public class MatchMakingViewBehaviour : MonoBehaviour
    {
        [SerializeField] private TMP_Text _usernameLabel = null;
        [SerializeField] private Button _connectButton = null;
        [SerializeField] private Button _backButton = null;
        
        private string _userName = String.Empty;
        private INetworkCore _networkingCore = null;
        
        public void Initialize(INetworkCore network)
        {
            _networkingCore = network;
            _connectButton.onClick.RemoveAllListeners();
            _connectButton.onClick.AddListener(() =>
            {
                SetLabelAsLoading();
                _networkingCore.Connect();
            });
            
            _networkingCore.SetUserName(_userName);
        }

        public void ShowView()
        {
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        private void HideView()
        {
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
        }

        private void SetLabelAsLoading()
        {
            _usernameLabel.text = "Loading...";
        }

        private void SetLabelAsUsername()
        {
            _usernameLabel.text = $"Username\n {_userName}";
        }
        
        private void Awake()
        {
            transform.localScale = Vector3.zero;
            _userName = UsernameGenerator.GeneratorRandomUserName();
            SetLabelAsUsername();
            _backButton.onClick.RemoveAllListeners();
            _backButton.onClick.AddListener(HideView);
        }
    }
}