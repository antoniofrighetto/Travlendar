using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Travlendar.Framework.ViewModels
{
    public abstract class BindableBaseNotify : INotifyPropertyChanged
    {
        //PropertyChanged event is raised whenever its property changes.
        public event PropertyChangedEventHandler PropertyChanged;

        //Notifies client that a property value has changed. CallerMemberName C# 5.0 attribute allows the property name of the caller to be substituted as argument.
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            //Create a local copy of PropertyChanged to avoid race conditions (a thread unsubscribe handler for the event after != null but before invoking PropertyChanged.
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //Set the property and notifies client just if it matches conditions.
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                this.RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}
