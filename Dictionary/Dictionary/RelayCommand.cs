﻿using System;
using System.Windows.Input;

namespace Dictionary
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> executeParam;
        private readonly Predicate<object> canExecuteParam;

        // Constructor for commands without parameters
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(execute != null ? new Action<object>(_ => execute()) : (Action<object>)null,
                   canExecute != null ? new Predicate<object>(_ => canExecute()) : (Predicate<object>)null)
        {
        }

        // Constructor for commands with parameters
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            executeParam = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecuteParam = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteParam == null || canExecuteParam(parameter);
        }

        public void Execute(object parameter)
        {
            executeParam?.Invoke(parameter);
        }
    }
}
