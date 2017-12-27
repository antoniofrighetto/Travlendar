namespace Travlendar.Core.Framework.Dependencies
{
    public interface IMessage
    {
        void LongAlert (string message);
        void ShortAlert (string message);
    }
}
