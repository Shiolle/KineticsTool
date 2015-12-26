using FireControl.Infrastructure;
using FireControl.Infrastructure.Interfaces;
using FireControl.Models.Implementation.LaunchBoard;
using FireControl.Models.Interfaces.LaunchBoard;
using FireControl.Models.Interfaces.TimeControl;
using FireControl.Models.Interfaces.UnitControl;
using FireControl.ViewModels.Avid;
using FireControl.ViewModels.DataContexts;
using FireControl.ViewModels.LaunchBoard;

namespace FireControl.ViewModels.Windows
{
    internal class LaunchBoardViewModel : ViewModelBase, INavigationNode
    {
        private const string TitleFormat = "Launch board {0} -> {1} at {2}";
        private const string EmptyTitle = "Launch board";

        private ILaunchBoardModel _launchBoardModel;

        private readonly ICurrentTurnModel _currentTurnModel;
        private readonly TargetingAvidViewModel _targetingAvid;
        private readonly LaunchPrerequisitesViewModel _launchPrerequisites;
        private readonly ShotGeometryTableViewModel _shotGeometry;
        private readonly RoCWorksheetViewModel _rocWorksheet;
        private readonly MpatViewModel _missilePositionAdjustment;
        private readonly MissileAccelerationViewModel _missileAcceleration;

        public LaunchBoardViewModel(ICurrentTurnModel currentTurn,
                                    LaunchPrerequisitesViewModel launchPrerequisites,
                                    TargetingAvidViewModel targetingAvid,
                                    ShotGeometryTableViewModel shotGeometry,
                                    RoCWorksheetViewModel rocWorksheet,
                                    MpatViewModel missilePositionAdjustment,
                                    MissileAccelerationViewModel missileAcceleration)
        {
            _currentTurnModel = currentTurn;

            _targetingAvid = targetingAvid;
            _launchPrerequisites = launchPrerequisites;
            _shotGeometry = shotGeometry;
            _rocWorksheet = rocWorksheet;
            _missilePositionAdjustment = missilePositionAdjustment;
            _missileAcceleration = missileAcceleration;
        }

        public string Title
        {
            get
            {
                return _launchBoardModel != null ?
                           string.Format(TitleFormat, _launchBoardModel.LaunchingUnit.Name, _launchBoardModel.Target.Name, _launchBoardModel.LaunchingSegment) :
                           EmptyTitle;
            }
        }

        public TargetingAvidViewModel TargetingAvid
        {
            get { return _targetingAvid; }
        }

        public LaunchPrerequisitesViewModel LaunchPrerequisites
        {
            get { return _launchPrerequisites; }
        }

        public ShotGeometryTableViewModel ShotGeometry
        {
            get { return _shotGeometry; }
        }

        public RoCWorksheetViewModel RoCWorksheet
        {
            get { return _rocWorksheet; }
        }

        public MpatViewModel MissilePositionAdjustment
        {
            get { return _missilePositionAdjustment; }
        }

        public MissileAccelerationViewModel MissileAcceleration
        {
            get { return _missileAcceleration; }
        }

        public void Initialize(object dataContext)
        {
            var context = VerifyContext<LaunchBoardDataContext>(dataContext);

            if (context != null)
            {
                SetupData(context.Attacker, context.Target);
            }
        }

        public void SetupData(IUnitModel attacker, IUnitModel target)
        {
            _launchBoardModel = new LaunchBoardModel(attacker, target, _currentTurnModel);
            _launchPrerequisites.Initialize(_launchBoardModel);
            _targetingAvid.Initialize(_launchBoardModel);
            _shotGeometry.Initialize(_launchBoardModel);
            _rocWorksheet.Initialize(_launchBoardModel);
            _missilePositionAdjustment.Initialize(_launchBoardModel);
            _missileAcceleration.Initialize(_launchBoardModel);

            OnPropertyChanged(Properties.Title);
        }

        private static class Properties
        {
            public const string Title = "Title";
        }
    }
}
