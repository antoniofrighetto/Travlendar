using Travlendar.AppCore.Navigator;

namespace Travlendar.Framework.ViewModels
{
    /// <summary>
    /// Interface that should be implemented by every viewmodel in the app.
    /// </summary>
    public interface IViewModel
    {
        INavigator Navigator { get; set; }
        /// <summary>
        /// Tag that represents this ViewModel ID for searching purpose in the application stack & fragment Manager
        /// </summary>
        /// <value>The tag.</value>
        string Tag { get; set; }
    }
}
