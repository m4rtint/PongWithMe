using System;
using UnityEngine;
using UnityEngine.UI;

namespace PongWithMe
{
    public class StartScreenViewBehaviour : MonoBehaviour
    {
        [SerializeField] private Button _localButton = null;
        [SerializeField] private Button _networkButton = null;

        public void Initialize(Action networkAction)
        {
            _networkButton.onClick.AddListener(() =>
            {
                networkAction();
            });
        }
    }
}