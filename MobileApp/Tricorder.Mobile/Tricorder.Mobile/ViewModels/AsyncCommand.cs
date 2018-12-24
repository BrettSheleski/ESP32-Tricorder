using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tricorder.Mobile.ViewModels
{
    public class AsyncCommand : Command
    {

        public AsyncCommand(Func<Task> asyncDelegate) : base(async() => await asyncDelegate())
        {
        }
    }
}