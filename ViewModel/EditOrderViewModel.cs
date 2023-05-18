using Dapper;
using MajorTest.Model;
using MajorTest.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MajorTest.ViewModel
{
    public class EditOrderViewModel : ViewModelBase
    {
        private string _adressSender;
        private string _adressReceiver;
        private double? _weight;
        private double? _length;
        private double? _width;
        private double? _height;
        private OrderDataRepository _orderDataRepository;
        private IEnumerable<OrderData> _orders;
        private int _selectedRow;

        public string AdressSender { get => _adressSender; set => _adressSender = value; }
        public string AdressReceiver { get => _adressReceiver; set => _adressReceiver = value; }
        public double? Weight { get => _weight; set => _weight = value; }
        public double? Length { get => _length; set => _length = value; }
        public double? Width { get => _width; set => _width = value; }
        public double? Height { get => _height; set => _height = value; }
        public IEnumerable<OrderData> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                OnPropertyChanged();
            }
        }
        public ICommand ShowOrderDataCommand { get; }
        public ICommand EditOrderDataCommand { get; }
        public ICommand DeleteOrderDataCommand { get; }
        public ICommand CancelOrderDataCommand { get; }
        public ICommand SubmitForExecutionCommand { get; }
        public ICommand CloseOrderCommand { get; }

        public bool test { get; set; } = true;
        public int SelectedRow { get => _selectedRow; set => _selectedRow = value; }

        public EditOrderViewModel()
        {
            SelectedRow = 0;
            _orderDataRepository = new OrderDataRepository();
            ShowOrderDataCommand = new ViewModelCommand(ExecuteShowOrderDataCommand);
            EditOrderDataCommand = new ViewModelCommand(ExecuteEditOrderDataCommand, CanExecuteToolCommand);
            DeleteOrderDataCommand = new ViewModelCommand(ExecuteDeleteOrderDataCommand, CanExecuteToolCommand);
            SubmitForExecutionCommand = new ViewModelCommand(ExecuteSubmitForExecutionCommand, CanExecuteToolCommand);
            CloseOrderCommand = new ViewModelCommand(ExecuteCloseOrderCommand, CanExecuteToolCommand);
            CancelOrderDataCommand = new ViewModelCommand(ExecuteCancelOrderCommand, CanExecuteToolCommand);
        }
        private void ExecuteCancelOrderCommand(object? obj)
        {
            string cancelReason = Microsoft.VisualBasic.Interaction.InputBox("Введите причину отмены:", "Причина отмены");
            if(String.IsNullOrEmpty(cancelReason))
            {
                MessageBox.Show("Изменения не внесены", "Причина отмены", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            foreach (var order in Orders)
            {
                order.CancelationReason = cancelReason;
                order.Status = "Отменено";
            }

            _orderDataRepository.CanselOrder(Orders.Where(x => x.Checker == true).ToArray());
            ShowOrderDataCommand.Execute(null);
        }
        private void ExecuteSubmitForExecutionCommand(object? obj)
        {
            _orderDataRepository.ChangeStatus(Orders.Where(x => x.Checker == true).Select(x => x.Id).ToArray(), "Передано на выполнение");
            ShowOrderDataCommand.Execute(null);
        }
        private void ExecuteCloseOrderCommand(object? obj)
        {
            _orderDataRepository.ChangeStatus(Orders.Where(x => x.Checker == true).Select(x => x.Id).ToArray(), "Выполнено");
            ShowOrderDataCommand.Execute(null);
        }

        private void ExecuteDeleteOrderDataCommand(object? obj)
        {
            _orderDataRepository.DeleteOrder(Orders.Where(x => x.Checker == true).Select(x => x.Id).ToArray());
            ShowOrderDataCommand.Execute(null);
        }

        private void ExecuteEditOrderDataCommand(object? obj)
        {
            _orderDataRepository.EditOrderData(Orders.Where(x => x.Checker == true).ToArray());
            ShowOrderDataCommand.Execute(null); ;
        }

        public void ExecuteShowOrderDataCommand(object? obj)
        {
            Orders = _orderDataRepository.GetFilteredCreateOrder(new FilterData 
            {
                AdressSender = AdressSender,
                AdressReceiver = AdressReceiver,
                Weight = Weight,
                Length = Length,
                Width = Width,
                Height = Height,
            });
            if(Orders.Count() == 0)
            {
                MessageBox.Show("В базе нет заказов", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CanExecuteToolCommand(object? arg) => SelectedRow != 0 ? true : false;
    }
}
