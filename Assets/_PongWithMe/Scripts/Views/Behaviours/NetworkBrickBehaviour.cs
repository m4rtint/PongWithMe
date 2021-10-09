using Photon.Pun;
using UnityEngine;

namespace PongWithMe
{
    public class NetworkBrickBehaviour : BrickBehaviour
    {
        protected override void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out BallBehaviour _))
            {
                PhotonView.Get(this).RPC("OnBrickBreak", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void OnBrickBreak()
        {
            _brick.IsActive = false;
        }
    }
}