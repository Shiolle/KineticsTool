using FireControl.Models.Interfaces.LaunchBoard;
using Kinetics.Core.Data.FiringSolution;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class MpatViewModel : ViewModelBase
    {
        private ILaunchBoardModel _launchBoardModel;
        private MissileAccelerationData _accelerationData;

        public void Initialize(ILaunchBoardModel launchBoardModel)
        {
            UnsubscribeFromEvents();
            _launchBoardModel = launchBoardModel;
            SubscribeToEvents();
            LaunchBoardModelOnFiringSolutionUpdated();
        }

        public bool DataAvailable
        {
            get { return _accelerationData != null && _launchBoardModel != null && _launchBoardModel.FiringSolution != null; }
        }

        public int TableColumn
        {
            get { return DataAvailable ? _accelerationData.TableColumn : 0; }
        }

        public int BurnDuration
        {
            get { return DataAvailable ? _accelerationData.BurnDuration : 0; }
        }

        public int TableRow
        {
            get { return DataAvailable ? _launchBoardModel.FiringSolution.ShotGeometryRow : 0; }
        }

        private void SubscribeToEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.FiringSolutionUpdated += LaunchBoardModelOnFiringSolutionUpdated;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (_launchBoardModel != null)
            {
                _launchBoardModel.FiringSolutionUpdated -= LaunchBoardModelOnFiringSolutionUpdated;
            }
        }

        private void LaunchBoardModelOnFiringSolutionUpdated()
        {
            _accelerationData = _launchBoardModel.AccelerationData;
            OnPropertyChanged(Properties.DataAvailable);
            OnPropertyChanged(Properties.TableRow);
            OnPropertyChanged(Properties.TableColumn);
            OnPropertyChanged(Properties.BurnDuration);
        }

        private static class Properties
        {
            public const string DataAvailable = "DataAvailable";
            public const string TableRow = "TableRow";
            public const string TableColumn = "TableColumn";
            public const string BurnDuration = "BurnDuration";
        }
    }
}
