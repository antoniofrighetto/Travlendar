using System.Threading.Tasks;
using Travlendar.Framework.Events;
using Xamarin.Forms;

namespace Travlendar.Framework.ViewModels
{
    public abstract class AViewModel<T> : IViewModel
    {
        private INavigation _navigation;
        public INavigation Navigation { get => _navigation; set => _navigation = value; }

        protected string _tag;

        public virtual string Tag
        {
            get
            {
                return _tag == null ? GetType ().Name : _tag;
            }
            set
            {
                if ( _tag != null && value != _tag )
                    _tag = value;
            }
        }

        protected T _model;

        public T Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
                RaiseOnModelChangedEvent ();
            }
        }

        public delegate void ModelDelegate (object obj, ModelEventArgs<T> args);

        public event ModelDelegate ModelChanged;

        protected void RaiseOnModelChangedEvent ()
        {
            if ( ModelChanged != null )
            {
                ModelChanged (this, new ModelEventArgs<T> (_model));
            }
        }

        /// <summary>
        /// Extends this for inits logics.
        /// </summary>
        public virtual void Start ()
        {
        }

        /// <summary>
        /// Extends this for inits logics in async way.
        /// </summary>
#pragma warning disable 1998
        public virtual async Task StartAsync ()
        {
        }
#pragma warning restore 1998
    }
}
