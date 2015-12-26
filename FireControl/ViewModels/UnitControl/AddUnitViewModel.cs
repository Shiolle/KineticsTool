using System.Globalization;
using System.Windows.Controls;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Implementation.UnitControl;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.Validation;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Storage;

namespace FireControl.ViewModels.UnitControl
{
    internal class AddUnitViewModel : ValidatedViewModelBase, INavigationNode
    {
        private readonly IUnitListModel _unitListModel;
        private readonly IStaticConfigurationController _configurationController;
        private readonly HexCoordinatesValidation _coordinatesValidation;

        private IUnitModel _newUnit;

        public AddUnitViewModel(IUnitListModel unitListModel, IStaticConfigurationController configurationController)
        {
            _unitListModel = unitListModel;
            _configurationController = configurationController;
            _coordinatesValidation = new HexCoordinatesValidation();
        }

        public bool Configured
        {
            get { return _newUnit != null; }
        }

        public string UnitName
        {
            get { return _newUnit != null ? _newUnit.Name : null; }
            set
            {
                _newUnit.Name = value;
                OnPropertyChanged(Properties.UnitName);
            }
        }

        public string Position { get; set; }

        public bool NoErrors { get; private set; }

        public void Initialize(object dataContext)
        {
            _newUnit = new UnitModel();

            if (!string.IsNullOrEmpty(_configurationController.StartingUnitPosition))
            {
                _newUnit.Position = HexGridCoordinate.Parse(_configurationController.StartingUnitPosition);
            }

            Position = _newUnit.Position.ToString();

            OnPropertyChanged(Properties.UnitName);
            OnPropertyChanged(Properties.Position);
            OnPropertyChanged(Properties.NoErrors);
            OnPropertyChanged(Properties.Configured);
        }

        public void AddUnit()
        {
            _newUnit.Position = HexGridCoordinate.Parse(Position);
            _unitListModel.AddUnit(_newUnit);
        }

        protected override ValidationResult TryValidateColumn(string columnName)
        {
            switch (columnName)
            {
                case Properties.UnitName:
                    if (string.IsNullOrEmpty(UnitName))
                    {
                        return new ValidationResult(false, "Please enter unit name.");
                    }
                    return new ValidationResult(true, null);
                case Properties.Position:
                    var validationResult = _coordinatesValidation.Validate(Position, CultureInfo.InvariantCulture);
                    return validationResult;
                default:
                    return new ValidationResult(true, null);
            }
        }

        protected override void OnValidationStatusChanged(bool hasErrors)
        {
            NoErrors = !hasErrors;
            OnPropertyChanged(Properties.NoErrors);
        }

        private static class Properties
        {
            public const string UnitName = "UnitName";
            public const string Position = "Position";
            public const string NoErrors = "NoErrors";
            public const string Configured = "Configured";
        }
    }
}
