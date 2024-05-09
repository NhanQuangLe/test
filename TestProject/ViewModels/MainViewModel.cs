using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {

            GetFlightsCommand = new RelayCommand(GetFlights);
        }

        #region Commands
        public RelayCommand GetFlightsCommand { get; private set; }
        public void GetFlights()
        {
            // TODO: Your logic here
            throw new OperationCanceledException();
        }
        #endregion
    }
}
