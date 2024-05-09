using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestProject.Helpers;

namespace TestProject.ViewModels
{
    public class ViewModelLocator
    {
        public static Uri FlightServiceURI = new Uri("https://interviewfe.eyeq.tech");

        static ViewModelLocator()
        {
            SimpleIoc.Default.Reset();
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);


            var httpClientHandlerAcceptAnyServerCertificateAndLogging = new HttpLoggingHandler(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
            SimpleIoc.Default.Register<FlightAPIService>(
               () => RefitExtensions.For<FlightAPIService>(
                   new HttpClient(httpClientHandlerAcceptAnyServerCertificateAndLogging) { BaseAddress = FlightServiceURI }));

            SimpleIoc.Default.Register<MainViewModel>(true);
        }
        #region Properties
        public static MainViewModel Main
        {
            get => ServiceLocator.Current.GetInstance<MainViewModel>();
        }
        #endregion

        #region Methods
        public static void Reregister(System.Type type)
        {
            System.Collections.Generic.Dictionary<System.Type, System.Delegate> actions = new System.Collections.Generic.Dictionary<System.Type, System.Delegate>{
                {typeof(MainViewModel), new System.Action(() =>
                    {
                        SimpleIoc.Default.Unregister<MainViewModel>();
                        SimpleIoc.Default.Register<MainViewModel>();
                    })
                },
        };
            if (actions.ContainsKey(type))
            {
                actions[type]?.DynamicInvoke();
            }
        }
        #endregion
    }
}
