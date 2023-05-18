using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MajorTest.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            }
            set
            {
                _currentChildView = value;
                OnPropertyChanged();
            }
        }
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged();
            }
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowCreateOrderViewCommand { get; }
        public ICommand ShowEditOrderViewCommand { get; }
        public MainViewModel()
        {
            ShowCreateOrderViewCommand = new ViewModelCommand(ExecuteShowCreateOrderViewCommand);
            ShowEditOrderViewCommand = new ViewModelCommand(ExecuteShowEditOrderViewCommand);

            ExecuteShowCreateOrderViewCommand(null);
        }

        private void ExecuteShowEditOrderViewCommand(object? obj)
        {
            CurrentChildView = new EditOrderViewModel();
            Caption = "Редактирование заказа";
            Icon = IconChar.FileEdit;
        }
        private void ExecuteShowCreateOrderViewCommand(object? obj)
        {
            CurrentChildView = new CreateOrderViewModel();
            Caption = "Создание заказа";
            Icon = IconChar.FileCirclePlus;
        }
    }
}
