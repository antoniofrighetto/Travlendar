using System;

namespace Travlendar.Framework.Events
{
    /// <summary>
    /// Model event arguments.
    /// </summary>
    public class ModelEventArgs<T> : EventArgs
    {
        public T Model { get; private set; }

        public ModelEventArgs (T model)
        {
            Model = model;
        }
    }
}
