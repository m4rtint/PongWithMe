namespace PongWithMe
{
    public interface IInput
    {
        bool IsPressingUp();
        bool IsPressingDown();
        bool IsPressingLeft();
        bool IsPressingRight();
    }
}