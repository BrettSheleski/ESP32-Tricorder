using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tricorder.Mobile.ViewModels
{
    public class BluetoothCharaccteristicDetailsViewModel : BaseViewModel
    {
        public BluetoothCharaccteristicDetailsViewModel()
        {
            this.ReadValueCommand = new AsyncCommand(ReadValueAsync);
            this.WriteValueCommand = new AsyncCommand(WriteValueAsync);
        }

        private async Task WriteValueAsync()
        {
            IsBusy = true;
            await Characteristic.SetValueAsync(this.Value);
            IsBusy = true;
        }

        private async Task ReadValueAsync()
        {
            IsBusy = true;
            this.Value = await Characteristic.GetValueAsync();
            IsBusy = false;
        }

        public IBluetoothCharacteristic Characteristic { get; set; }

        private byte[] _value;
        public byte[] Value { get => _value; set => SetProperty(ref _value, value); }
        public AsyncCommand ReadValueCommand { get; }
        public AsyncCommand WriteValueCommand { get; }

        public override Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

    }
}
