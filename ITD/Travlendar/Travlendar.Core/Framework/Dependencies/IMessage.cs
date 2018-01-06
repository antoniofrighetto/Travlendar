namespace Travlendar.Core.Framework.Dependencies
{
    /// <summary>
    /// Interface for the Toast Notification
    /// </summary>
    public interface IMessage
    {
        void LongAlert (string message);
        void ShortAlert (string message);
    }
}
