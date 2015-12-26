using System;
using System.ComponentModel;

namespace FireControl.ViewModels
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected T VerifyContext<T>(object dataContext) where T : class
        {
            if (dataContext == null)
            {
                return default(T);
            }

            var context = dataContext as T;

            if (context == null)
            {
                throw new ArgumentException(string.Format("Data context cannot be converted to type {0}", typeof(T)));
            }

            return context;
        }
    }
}
