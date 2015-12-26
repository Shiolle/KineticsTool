using FireControl.Models.Interfaces.LaunchBoard;
using Kinetics.Core.Data.FiringSolution;

namespace FireControl.ViewModels.LaunchBoard
{
    internal class ShotGeometryTableViewModel : ViewModelBase
    {
        private ILaunchBoardModel _launchBoardModel;
        private FiringSolution _firingSolution;

        public void Initialize(ILaunchBoardModel launchBoardModel)
        {
            UnsubscribeFromEvents();
            _launchBoardModel = launchBoardModel;
            SubscribeToEvents();
            LaunchBoardModelOnFiringSolutionUpdated();
        }

        public bool DataAvailable
        {
            get { return _firingSolution != null; }
        }

        public int TableRow
        {
            get { return _firingSolution != null ? _firingSolution.ShotGeometryRow : 0; }
        }

        public int TableColumn
        {
            get { return _firingSolution != null ? _firingSolution.ShotGeometryColumn : 0; }
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
            _firingSolution = _launchBoardModel.FiringSolution;
            OnPropertyChanged(Properties.DataAvailable);
            OnPropertyChanged(Properties.TableRow);
            OnPropertyChanged(Properties.TableColumn);
        }

        private static class Properties
        {
            public const string DataAvailable = "DataAvailable";
            public const string TableRow = "TableRow";
            public const string TableColumn = "TableColumn";
        }
    }
}
