namespace PongWithMe
{
    public class PortalsMutator : BaseMutator
    {
        private Portals _portals = null;

        public override string Announcement => "PORTALS";

        public PortalsMutator(Portals portals)
        {
            _portals = portals;
        }

        public override void ActivateMutator()
        {
            _portals.ActivatePortals();
        }

        public override bool CanActivate()
        {
            return !_portals.ArePortalsActive;
        }
    }
}