using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace FireControl.ViewModels
{
    /// <summary>
    /// A base view model that has a number of fields with validation, and it needs to know if all of them have been validated successfully.
    /// </summary>
    internal abstract class ValidatedViewModelBase : ViewModelBase, IDataErrorInfo
    {
        private readonly List<string> _columnsWithValidationErrors = new List<string>();

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                var result = TryValidateColumn(columnName);

                if (result == null)
                {
                    return null;
                }

                if (result.IsValid)
                {
                    OnValidationSucceeded(columnName);
                }
                else
                {
                    OnValidationFailed(columnName);
                }

                return result.ErrorContent != null ? result.ErrorContent.ToString() : null;
            }
        }

        /// <summary>
        /// Try validate the specified column.
        /// </summary>
        /// <param name="columnName">Validated column name.</param>
        /// <returns>True if validation was successfull; otherwise false. Also contains error content, if any.</returns>
        protected abstract ValidationResult TryValidateColumn(string columnName);

        /// <summary>
        /// Handles a situation where last error has been fixed or first error appears.
        /// </summary>
        /// <param name="hasErrors">New validation status.</param>
        protected abstract void OnValidationStatusChanged(bool hasErrors);

        private void OnValidationSucceeded(string columnName)
        {
            if (_columnsWithValidationErrors.Contains(columnName))
            {
                // We need to check if we got rid of the last error;
                bool lastError = _columnsWithValidationErrors.Count == 1;
                _columnsWithValidationErrors.Remove(columnName);
                if (lastError)
                {
                    OnValidationStatusChanged(false);
                }
            }
        }

        private void OnValidationFailed(string columnName)
        {
            if (!_columnsWithValidationErrors.Contains(columnName))
            {
                // Do we have first error?
                bool firstError = _columnsWithValidationErrors.Count == 0;
                _columnsWithValidationErrors.Add(columnName);
                if (firstError)
                {
                    OnValidationStatusChanged(true);
                }
            }
        }
    }
}
