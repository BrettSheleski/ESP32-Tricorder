using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tricorder.Mobile.ViewModels
{
    public class TricorderViewModel : BaseViewModel
    {
        public TricorderViewModel()
        {
            this.BluetoothManager = Xamarin.Forms.DependencyService.Get<IBluetoothManager>();

            this.UpdateDeviceListCommand = new AsyncCommand(UpdateDeviceListAsync);
            this.ShowMenuCommand = new Command(x => IsMenuDisplayed = true);
        }

        private bool _isUpdatingDeviceList = false;

        private async Task UpdateDeviceListAsync()
        {
            IsUpdatingDeviceList = true;
            var devices = await BluetoothManager.GetDevicesAsync();

            foreach (var device in devices.OrderBy(x => x.Name))
            {
                if (true || await IsDeviceTricorderAsync(device))
                {
                    AvailableDevices.Add(device);
                }
            }

            if (AvailableDevices.Count > 0 && CurrentDevice == null)
            {
                CurrentDevice = AvailableDevices[0];
            }
            IsUpdatingDeviceList = false;
        }

        private IBluetoothDevice _currentDevice;

        public IBluetoothDevice CurrentDevice
        {
            get => _currentDevice; set
            {
                SetProperty(ref _currentDevice, value);
                if (value != null)
                {
                    OnCurrentDeviceChanged(value);
                }
            }
        }

        private void OnCurrentDeviceChanged(IBluetoothDevice value)
        {
            IsMenuDisplayed = false;
        }

        public ObservableCollection<IBluetoothDevice> AvailableDevices { get; } = new ObservableCollection<IBluetoothDevice>();
        public IBluetoothManager BluetoothManager { get; }
        public AsyncCommand UpdateDeviceListCommand { get; }
        public Command ShowMenuCommand { get; }
        public bool IsUpdatingDeviceList { get => _isUpdatingDeviceList; set => SetProperty(ref _isUpdatingDeviceList, value); }
        public bool IsMenuDisplayed { get => _isMenuDisplayed; set => SetProperty(ref _isMenuDisplayed, value); }
        public int Fart { get => _fart; set => SetProperty(ref _fart, value); }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await UpdateDeviceListAsync();
        }

        private async Task<bool> IsDeviceTricorderAsync(IBluetoothDevice device)
        {
            var services = await device.GetServicesAsync();

            return services.Any(s => s.Id == Tricorder.Bluetooth.TricorderServiceId);
        }

        private bool _isMenuDisplayed;

        private int _fart;
    }
}
