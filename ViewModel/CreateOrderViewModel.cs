using MajorTest.Model;
using MajorTest.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MajorTest.ViewModel
{
    public class CreateOrderViewModel : ViewModelBase
    {
        private string _adressSender;
        private string _adressReceiver;
        private double? _weight;
        private double? _length;
        private double? _width;
        private double? _height;
        private string _errorMessage;
        private OrderDataRepository _orderDataRepository;

        public string AdressSender { get => _adressSender; set => _adressSender = value; }
        public string AdressReceiver { get => _adressReceiver; set => _adressReceiver = value; }
        public double? Weight { get => _weight; set => _weight = value; }
        public double? Length { get => _length; set => _length = value; }
        public double? Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public double? Height { get => _height; set => _height = value; }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SendOrderDataCommand { get; }
        public CreateOrderViewModel()
        {
            _orderDataRepository = new OrderDataRepository();
            SendOrderDataCommand = new ViewModelCommand(ExecuteSendOrderDataCommand, CanExecuteSendOrderDataCommand);
        }

        private bool CanExecuteSendOrderDataCommand(object? arg)
        {
            bool validData = true;
            if (string.IsNullOrWhiteSpace(AdressSender) || string.IsNullOrWhiteSpace(AdressReceiver)
                || string.IsNullOrWhiteSpace(Weight.ToString()) || string.IsNullOrWhiteSpace(Length.ToString())
                || string.IsNullOrWhiteSpace(Height.ToString()) || string.IsNullOrWhiteSpace(Width.ToString()))
                {
                    validData = false;
                }
            return validData;
        }

        private void ExecuteSendOrderDataCommand(object? obj)
        {
            ErrorMessage = "";
            int d = 0;

            _orderDataRepository.CreateOrder(new OrderData
            {
                AdressSender = AdressSender,
                AdressReceiver = AdressReceiver,
                Weight = Weight,
                Length = Length,
                Width = Width,
                Height = Height,
                Volume = Length.Value * Width.Value * Height.Value,
                Status = "Новая",
            });
            MessageBox.Show("Заявка успешно создана", "Создание заявки", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
