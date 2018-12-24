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
            
            MainPage = new NavigationPage(new BluetoothDeviceListPage
            {
                BindingContext = new ViewModels.BluetoothDeviceListViewModel()
            });
        }


    }
}