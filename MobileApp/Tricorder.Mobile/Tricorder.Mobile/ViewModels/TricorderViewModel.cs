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
            this.UpdateFartCommand = new AsyncCommand(UpdateFartAsync);
        }


        IBluetoothCharacteristic _fartCharacteristic;

        private async Task UpdateFartAsync()
        {
            if (FartCharacteristic != null)
            {
                byte[] fartBytes = await FartCharacteristic.GetValueAsync();

                byte[] bytesCopy = new byte[fartBytes.Length];

                fartBytes.CopyTo(bytesCopy, 0);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytesCopy);

                Fart = BitConverter.ToInt32(bytesCopy, 0);
            }
        }

        private bool _isUpdatingDeviceList = false;

        private async Task UpdateDeviceListAsync()
        {
            IsUpdatingDeviceList = true;

            AvailableDevices.Clear();
            CurrentDevice = null;

            var devices = await BluetoothManager.GetDevicesAsync();

            Task<IBluetoothService[]>[] tasks = devices.Select(d => d.GetServicesAsync()).ToArray();

            await Task.WhenAll(tasks);

            for (int i = 0; i < tasks.Length; ++i)
            {
                if (tasks[i].Result.Any(s => s.Id == Bluetooth.TricorderServiceId))
                {
                    AvailableDevices.Add(devices[i]);
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

        private async void OnCurrentDeviceChanged(IBluetoothDevice value)
        {
            IsMenuDisplayed = false;
            var services = await value.GetServicesAsync();

            foreach (var service in services)
            {
                var characteristics = await service.GetCharacteristicsAsync();

                FartCharacteristic = characteristics.Where(c => c.Id == Tricorder.Bluetooth.FartSensorCharacteristicId).FirstOrDefault();

                if (FartCharacteristic != null)
                {
                    return;
                }
            }

            FartCharacteristic = null;

        }

        public ObservableCollection<IBluetoothDevice> AvailableDevices { get; } = new ObservableCollection<IBluetoothDevice>();
        public IBluetoothManager BluetoothManager { get; }
        public AsyncCommand UpdateDeviceListCommand { get; }
        public Command ShowMenuCommand { get; }
        public AsyncCommand UpdateFartCommand { get; }
        public bool IsUpdatingDeviceList { get => _isUpdatingDeviceList; set => SetProperty(ref _isUpdatingDeviceList, value); }
        public bool IsMenuDisplayed
        {
            get => _isMenuDisplayed;
            set => SetProperty(ref _isMenuDisplayed, value);
        }
        public int Fart { get => _fart; set => SetProperty(ref _fart, value); }
        public IBluetoothCharacteristic FartCharacteristic { get => _fartCharacteristic; set => SetProperty(ref _fartCharacteristic, value); }
        public DateTime Clock { get => _clock; set => SetProperty(ref _clock, value); }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await UpdateDeviceListAsync();

            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), TimerTick);
        }

        private bool TimerTick()
        {
            Task.Run(UpdateAsync);

            return true;
        }

        private async Task UpdateAsync()
        {
            await UpdateFartAsync();

            Clock = DateTime.Now;
        }

        private DateTime _clock = DateTime.Now;

        private async Task<bool> IsDeviceTricorderAsync(IBluetoothDevice device)
        {
            var services = await device.GetServicesAsync();

            return services.Any(s => s.Id == Tricorder.Bluetooth.TricorderServiceId);
        }

        private bool _isMenuDisplayed;

        private int _fart;
    }
}
