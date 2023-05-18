﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace MajorTest.ViewModel
{
    class ViewModelCommand : ICommand
    {
        private readonly Action<object> _execute;
        private Func<object?, bool> _canExecute;

        public ViewModelCommand(Action<object?> execute, Func<object?, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
