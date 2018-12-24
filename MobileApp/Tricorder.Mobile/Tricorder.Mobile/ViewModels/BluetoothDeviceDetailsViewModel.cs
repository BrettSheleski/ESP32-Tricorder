using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tricorder.Mobile.ViewModels
{
    public class BluetoothDeviceDetailsViewModel : BaseViewModel
    {
        public BluetoothDeviceDetailsViewModel()
        {
            this.UpdateServicesCommand = new AsyncCommand(UpdateServicesAsync);
        }

        private async Task UpdateServicesAsync()
        {
            Services.Clear();
            ServiceInfo serviceInfo;
            foreach (var service in await Device.GetServicesAsync())
            {
                serviceInfo = new ServiceInfo
                {
                    Service = service
                };

                serviceInfo.Characteristics.AddRange(await service.GetCharacteristicsAsync());

                this.Services.Add(serviceInfo);
            }
        }

        public IBluetoothDevice Device { get; set; }

        public ObservableCollection<ServiceInfo> Services { get; } = new ObservableCollection<ServiceInfo>();
        public AsyncCommand UpdateServicesCommand { get; }
        public IBluetoothCharacteristic SelectedCharacteristic
        {
            get => _selectedCharacteristic;
            set
            {
                _selectedCharacteristic = value;
                OnPropertyChanged();

                if (value != null)
                {
                    NavigateToAsync(value);

                    SelectedCharacteristic = null;
                }
            }
        }

        private async void NavigateToAsync(IBluetoothCharacteristic value)
        {
            if (App.Current.MainPage is NavigationPage navPage)
            {
                var vm = new BluetoothCharaccteristicDetailsViewModel
                {
                    Characteristic = value
                };

                await navPage.PushAsync(new Views.BluetoothCharacteristicPage
                {
                    BindingContext = vm
                });

                await vm.InitializeAsync();
            }
        }

        public async Task InitializeAsync()
        {
            await UpdateServicesAsync();
        }

        public class ServiceInfo : List<IBluetoothCharacteristic>
        {
            public IBluetoothService Service { get; set; }

            public List<IBluetoothCharacteristic> Characteristics { get { return this; } }
        }

        private IBluetoothCharacteristic _selectedCharacteristic;
    }
}
