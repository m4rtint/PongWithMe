using System;
using System.Collections.Generic;
using PerigonGames;
using UnityEngine;

namespace PongWithMe
{
    public class Portals
    {
        private readonly float _timeToLive = 5;
        private readonly Vector3[] _portalPositions;
        private readonly IRandomUtility _random;

        private float _elapsedTime = 0;
        private bool _arePortalsActive = false;

        public event Action OnTeleport;
        
        private List<Portal> _listOfPortals = new List<Portal>();
        public List<Portal> ListOfPortals => _listOfPortals;

        public bool ArePortalsActive => _arePortalsActive;
        
        public Portals(Vector3[] portalPositions, float timeToLive,  IRandomUtility randomUtility = null)
        {
            _portalPositions = portalPositions;            
            _timeToLive = timeToLive;
            _random = randomUtility ?? new RandomUtility();
            SetupPortals(portalPositions.Length);

        }
        
        private void SetupPortals(int numberOfPortals)
        {
            for (int i = 0; i < numberOfPortals; i++)
            {
                var portal = new Portal(i);
                portal.OnBallEnteredPortal += HandleOnTeleport;
                _listOfPortals.Add(portal);
            }
        }
        
        public void CleanUp()
        {
            _arePortalsActive = false;
            _elapsedTime = 0;
            _listOfPortals.ForEach(portal => portal.CleanUp());
        }

        public void Reset()
        {
            _elapsedTime = _timeToLive;
        }

        public void ActivatePortals()
        {
            _elapsedTime = _timeToLive;
            _arePortalsActive = true;
            _listOfPortals.ForEach(portal => portal.IsActive = true);
        }        
        
        public void AllowTeleport()
        {
            _listOfPortals.ForEach(portal => portal.CanTeleport = true);
        }

        public void Update(float deltaTime)
        {
            if (!_arePortalsActive)
            {
                return;
            } 
            
            _elapsedTime -= deltaTime;
            if (_elapsedTime < 0)
            {
                DeactivatePortals();
            }
        }

        private void DeactivatePortals()
        {
            _arePortalsActive = false;
            _listOfPortals.ForEach(portal => portal.IsActive = false);
        }
        
        private void DisallowTeleport()
        {
            _listOfPortals.ForEach(portal => portal.CanTeleport = false);
        }
        
        private void HandleOnTeleport(IBall ball, int index)
        {
            DisallowTeleport();
            ball.SetPosition(GetRandomPortalLocation(index));
            OnTeleport?.Invoke();
        }
        
        private Vector3 GetRandomPortalLocation(int index)
        {
            var randomPortalIndex = RandomPortal(index);
            return _portalPositions[randomPortalIndex];
        }
        
        private int RandomPortal(int index)
        {
            var nextIndex = index;
            while(nextIndex == index)
            {
                nextIndex = _random.NextInt(0, _listOfPortals.Count);
            }

            return nextIndex;
        }
    }
}

