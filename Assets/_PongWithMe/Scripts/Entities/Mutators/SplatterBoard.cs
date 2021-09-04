
namespace PongWithMe
{
    public class SplatterBoard : BaseMutator
    {
        private Splatter _splatter = null;

        public override string Announcement => "SPLATTER";

        public SplatterBoard(Splatter splatter)
        {
            _splatter = splatter;
        }

        public override void ActivateMutator()
        {
            _splatter.ActivateSplatters();
        }
    }
}