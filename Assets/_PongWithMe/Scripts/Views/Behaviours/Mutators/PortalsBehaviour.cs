using System.Collections;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace PongWithMe
{
    public class PortalsBehaviour : MonoBehaviour
    {
        [SerializeField] private PortalBehaviour[] _portalBehaviours =  null;
        [SerializeField] private float _timeToLive = 10f;
        
        private Portals _portals = null;
        public Portals Portals => _portals;
        
        public void Initialize(Portals portal = null)
        {
            var positions = _portalBehaviours.Select(portal => portal.transform.position).ToArray();
            _portals = portal ?? new Portals(positions, _timeToLive);
            SetupListOfPortals();
        }

        private void HandleOnTeleport()
        {
            StartCoroutine(Reactivate());
        }

        public void CleanUp()
        {
            _portals.OnTeleport -= HandleOnTeleport;
            _portals.CleanUp();
        }

        public void Reset()
        {
            _portals.OnTeleport += HandleOnTeleport;
            _portals.Reset();
        }

        private void SetupListOfPortals()
        {
            for (int i = 0; i < _portalBehaviours.Length; i++)
            {
                var portal = _portals.ListOfPortals[i];
                _portalBehaviours[i].Initialize(portal);
            }
        }

        private IEnumerator Reactivate()
        {
            yield return new WaitForSeconds(0.5f);
            _portals.AllowTeleport();
        }

        private void Update()
        {
            _portals.Update(Time.deltaTime);
        }
    }


}
