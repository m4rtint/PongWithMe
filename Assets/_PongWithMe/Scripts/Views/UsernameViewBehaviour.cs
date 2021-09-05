using System;
using TMPro;
using UnityEngine;

namespace PongWithMe
{
    public class UsernameViewBehaviour : MonoBehaviour
    {
        private TMP_Text _usernameLabel = null;
        
        private string _userName = String.Empty;

        public string UserName => _userName;

        public void SetLabelAsLoading()
        {
            _usernameLabel.text = "Loading...";
        }

        public void SetLabelAsUsername()
        {
            _usernameLabel.text = $"Username\n {_userName}";
        }
        
        private void Awake()
        {
            _usernameLabel = GetComponent<TMP_Text>();
            _userName = UsernameGenerator.GeneratorRandomUserName();
        }

        private void Start()
        {
            SetLabelAsUsername();
        }
    }
}