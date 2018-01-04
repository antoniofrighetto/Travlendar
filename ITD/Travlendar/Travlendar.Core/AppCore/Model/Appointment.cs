using System;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace Travlendar.Core.AppCore.Model
{
    public class Appointment : Telerik.XamarinForms.Input.IAppointment
    {
        public string Key { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public bool IsAllDay { get; set; }
        public string Detail { get; set; }
        public Color Color { get; set; }
    }
}
