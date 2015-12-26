using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace FireControl.Commands
{
    internal class SimpleCommand : Freezable, ICommand
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(object), typeof(SimpleCommand), new FrameworkPropertyMetadata(null, OnTargetChanged));

        public static readonly DependencyProperty IsReadyProperty =
            DependencyProperty.Register("IsReady", typeof(bool), typeof(SimpleCommand), new PropertyMetadata(true, OnReadyChanged));

        [Bindable(true)]
        public object Target
        {
            get { return GetValue(TargetProperty); }
            set
            {
                SetValue(TargetProperty, value);
            }
        }

        public string MethodName { get; set; }

        public bool IsReady
        {
            get { return (bool)GetValue(IsReadyProperty); }
            set
            {
                SetValue(IsReadyProperty, value);
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsReady;
        }

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object parameter)
        {
            object[] parameters = parameter != null ? new[] { parameter } : null;
            InvokeMethod(parameters);
        }

        protected void InvokeMethod(object[] parameters)
        {
            if (Target != null && !string.IsNullOrEmpty(MethodName))
            {
                Type type = Target.GetType();
                MethodInfo method = type.GetMethod(MethodName);
                method.Invoke(Target, parameters);
            }
        }

        private static void OnReadyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            EventHandler onCanExecuteChanged = ((SimpleCommand)dependencyObject).CanExecuteChanged;
            if (onCanExecuteChanged != null)
            {
                onCanExecuteChanged(dependencyObject, new EventArgs());
            }
        }

        private static void OnTargetChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((SimpleCommand)dependencyObject).TargetChanged();
        }

        protected virtual void TargetChanged() { }

        protected override Freezable CreateInstanceCore()
        {
            return new SimpleCommand();
        }
    }
}
