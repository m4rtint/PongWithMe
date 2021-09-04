using System.Collections;
using DG.Tweening;
using PerigonGames;
using Sirenix.Utilities;
using UnityEngine;

namespace PongWithMe
{
    public class TeleportersMutatorBehaviour : MonoBehaviour
    {
        [SerializeField] private TeleporterBehaviour[] _teleporters =  null;

        private void Awake()
        {
            for (int i = 0; i < _teleporters.Length; i++)
            {
                _teleporters[i].Initialize(i);
                _teleporters[i].OnTeleportStart += HandleOnTeleport;
            }
        }

        private void HandleOnTeleport(BallBehaviour ball, int index)
        {
            _teleporters.ForEach(teleport => teleport.IsActive = false);
            var location = _teleporters[RandomTeleporter(index)].transform.position;
            ball.transform.position = location;
            StartCoroutine(Reactivate());
        }

        private IEnumerator Reactivate()
        {
            yield return new WaitForSeconds(0.5f);
            _teleporters.ForEach(teleport => teleport.IsActive = true);

        }

        private int RandomTeleporter(int index)
        {
            var nextIndex = index;
            var random = new RandomUtility();
            while(nextIndex == index)
            {
                nextIndex = random.NextInt(0, _teleporters.Length);
            }

            return nextIndex;
        }
    }
}
