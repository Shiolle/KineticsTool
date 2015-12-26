using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Interfaces.LaunchBoard;
using FireControl.Properties;
using FireControl.ViewModels.AvidElementBoard;
using FireControl.ViewModels.DataContexts;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexVectors;

namespace FireControl.ViewModels.Avid
{
    internal class TargetingAvidViewModel : ViewModelBase
    {
        private const int VectorsCategory = 1;
        private const int LaunchWindowsCategory = 2;

        private ILaunchBoardModel _model;
        private readonly INavigationService _navigationService;
        private readonly AvidElementBoardViewModel _elementBoard;

        private IAvidMark _targetVectorMark;
        private IAvidMark _crossingVectorMark;

        public TargetingAvidViewModel(INavigationService navigationService, AvidElementBoardViewModel elementBoard)
        {
            _navigationService = navigationService;
            _elementBoard = elementBoard;
            CreateComponentModels();
        }

        public void Initialize(ILaunchBoardModel launchBoardModel)
        {
            UnsubscribeFromEvents();
            _model = launchBoardModel;
            SubscribeToEvents();
            CreateOrUpdateVelocityVectors(launchBoardModel.TargetDistance, launchBoardModel.CrossingVector);
            _model.UpdatePlatforms();
        }

        public bool DataAvailable
        {
            get { return _model != null; }
        }

        public VectorComponentViewModel[] LaunchingUnitVelocity { get; private set; }
        public VectorComponentViewModel[] TargetUnitVelocity { get; private set; }

        public AvidElementBoardViewModel ElementBoard
        {
            get { return _elementBoard; }
        }

        public int TargetDistance
        {
            get { return _model != null && _model.TargetDistance != null ? _model.TargetDistance.Magnitude : 0; }
        }

        public void UpdateVectors()
        {
            if (_model != null)
            {
                _model.UpdatePlatforms();
            }
        }

        private void SubscribeToEvents()
        {
            if (_model == null)
            {
                return;
            }

            _model.VectorsUpdated += ModelOnVectorsUpdated;
            _model.BearingVerificationRequired += ModelOnBearingVerificationRequired;
            _model.FiringSolutionUpdated += ModelOnFiringSolutionUpdated;
        }

        private void UnsubscribeFromEvents()
        {
            if (_model == null)
            {
                return;
            }

            _model.VectorsUpdated -= ModelOnVectorsUpdated;
            _model.BearingVerificationRequired -= ModelOnBearingVerificationRequired;
            _model.FiringSolutionUpdated -= ModelOnFiringSolutionUpdated;
        }

        private void ModelOnVectorsUpdated()
        {
            // Update vector components.
            for (int i = 0; i < 8; i++)
            {
                LaunchingUnitVelocity[i].UpdateVector(_model.LaunchingUnitVelocity);
                TargetUnitVelocity[i].UpdateVector(_model.TargetUnitVelocity);
            }

            CreateOrUpdateVelocityVectors(_model.TargetDistance, _model.CrossingVector);

            OnPropertyChanged(Properties.TargetDistance);
            OnPropertyChanged(Properties.DataAvailable);
        }

        private void ModelOnBearingVerificationRequired()
        {
            var context = new WindowSelectionContext
            {
                CanSelectDirection = true,
                CanSelectRing = true,
                InitialWindow = _model.TargetDistance,
                Caption = Resources.TargetingAvidViewModel_VerifyBearingCaption,
                Message = Resources.TargetingAvidViewModel_BearingVerificationMessage
            };

            _model.SubmitBearingVerificationResult(_navigationService.SelectWindow(context));
        }

        private void ModelOnFiringSolutionUpdated()
        {
            _elementBoard.WipeCategory(LaunchWindowsCategory);
            _elementBoard.AddMarks(LaunchWindowsCategory, "L", true, false, _model.LaunchWindows);
        }

        private void CreateOrUpdateVelocityVectors(AvidVector targetVector, AvidVector crossingVector)
        {
            CreateOrUpdateVector(targetVector, ref _targetVectorMark, VectorsCategory, false);
            CreateOrUpdateVector(crossingVector, ref _crossingVectorMark, VectorsCategory, true);
        }

        private void CreateOrUpdateVector(AvidVector originalVector, ref IAvidMark targetVector, int categoryId, bool underlined)
        {
            string markText = originalVector != null ? originalVector.Magnitude.ToString("D") : string.Empty;
            AvidWindow position = originalVector ?? new AvidWindow();

            if (targetVector == null)
            {
                targetVector = _elementBoard.AddMark(categoryId, markText, originalVector != null, underlined, position);
            }
            else
            {
                targetVector.Text = markText;
                targetVector.Window = position;
                targetVector.Visible = !AvidWindow.IsNullOrZero(position);
            }
        }

        private void CreateComponentModels()
        {
            LaunchingUnitVelocity = new VectorComponentViewModel[8];
            TargetUnitVelocity = new VectorComponentViewModel[8];

            for (int i = 0; i < 8; i++)
            {
                var hexAxis = (HexAxis)i + 1;
                LaunchingUnitVelocity[i] = new VectorComponentViewModel(hexAxis);
                TargetUnitVelocity[i] = new VectorComponentViewModel(hexAxis);
            }
        }

        private static class Properties
        {
            public const string TargetDistance = "TargetDistance";
            public const string DataAvailable = "DataAvailable";
        }
    }
}
