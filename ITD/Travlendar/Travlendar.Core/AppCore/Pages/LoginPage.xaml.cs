using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travlendar.Core.AppCore.Helpers;
using Travlendar.Core.AppCore.Renderers;
using Travlendar.Core.AppCore.ViewModels;
using Xamarin.Forms;

namespace Travlendar.Core.AppCore.Pages
{
    
    public partial class LoginPage : ContentPage
    {

        LoginViewModel loginViewModel;
        public LoginPage(LoginViewModel loginVM)
        {
            loginViewModel = loginVM;
            InitializeComponent();
        }
    }
}