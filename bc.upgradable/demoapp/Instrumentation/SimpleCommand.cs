using System;
using System.Windows.Input;

namespace demoapp.Instrumentation
{
    public class SimpleCommand : ICommand
    {
        public SimpleCommand(Action<object?> action)
        {
            _onExecute = action;
        }
        public event EventHandler? CanExecuteChanged;
        public Predicate<object?> CanExecutePredicate { get; set; } = (_) => true;

        private Action<object?>? _onExecute { get; set; } = null;

        public bool CanExecute(object? parameter)
        {
            return CanExecutePredicate(parameter);
        }

        public void Execute(object? parameter)
        {
            if (CanExecute(parameter) && _onExecute != null)
            {
                _onExecute(parameter);
            }
        }
    }
}
