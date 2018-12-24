using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Tricorder.Mobile.Views;
using Xamarin.Forms;

namespace Tricorder.Mobile.ViewModels
{
    public class BluetoothDeviceListViewModel : BaseViewModel
    {
        public BluetoothDeviceListViewModel()
        {
            this.BluetoothManager = Xamarin.Forms.DependencyService.Get<IBluetoothManager>();
            this.UpdateDevicesCommand = new AsyncCommand(UpdateDevicesAsync);
        }

        private async Task UpdateDevicesAsync()
        {
            IsBusy = true;

            Devices.Clear();

            foreach (var device in await BluetoothManager.GetDevicesAsync())
            {
                Devices.Add(device);
            }

            IsBusy = false;
        }

        public ObservableCollection<IBluetoothDevice> Devices { get; } = new ObservableCollection<IBluetoothDevice>();
        public IBluetoothManager BluetoothManager { get; }
        public AsyncCommand UpdateDevicesCommand { get; }
        public IBluetoothDevice SelectedDevice
        {
            get => _selectedDevice;
            set {
                SetProperty(ref _selectedDevice, value);
                if (value != null)
                {
                    NavigateToAsync(value);
                    SelectedDevice = null;
                }
            }
        }

        private async void NavigateToAsync(IBluetoothDevice value)
        {
            if (App.Current.MainPage is NavigationPage navPage)
            {
                var vm = new BluetoothDeviceDetailsViewModel
                {
                    Device = value
                };

                await navPage.PushAsync(new BluetoothDeviceDetailsPage
                {
                    BindingContext = vm
                });

                await vm.InitializeAsync();
            }

        }

        private IBluetoothDevice _selectedDevice;
    }
}
