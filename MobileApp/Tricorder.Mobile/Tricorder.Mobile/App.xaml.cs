using System;
using Xamarin.Forms;
using Tricorder.Mobile.Views;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tricorder.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TricorderPage
            {
                BindingContext = new ViewModels.TricorderViewModel()
            };
        }

        protected async override void OnStart()
        {
            base.OnStart();
            
            if (MainPage?.BindingContext is ViewModels.BaseViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }

    }
}