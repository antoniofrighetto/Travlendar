using System;

namespace Travlendar.Core.AppCore.Helpers
{
    /// <summary>
    /// TicketEventArgs custom Event which will handle the path of the saved Tickets.
    /// </summary>
    class TicketEventArgs : EventArgs
    {
        public string Path { get; set; }
    }
}
