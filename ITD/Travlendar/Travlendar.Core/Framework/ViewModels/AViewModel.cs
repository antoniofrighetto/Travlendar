using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Travlendar.Framework.ViewModels
{
    /// <summary>
    /// Base class from ViewModels
    /// </summary>
    public abstract class AViewModel<T> : IViewModel, INotifyPropertyChanged
    {
        private INavigation _navigation;
        public INavigation Navigation { get => _navigation; set => _navigation = value; }

        public abstract event PropertyChangedEventHandler PropertyChanged;

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
