using System;
using FireControl.Models.Implementation.ShellStars;
using FireControl.Models.Interfaces.LaunchBoard;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.TimeControl;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core;
using Kinetics.Core.Data;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.HexVectors;
using Kinetics.Core.Data.RefData;
using Kinetics.Core.Interfaces.Calculators;

namespace FireControl.Models.Implementation.LaunchBoard
{
    internal class LaunchBoardModel : ILaunchBoardModel
    {
        private const string DefaultTagFormat = "{0} launhed on {1} from {2}";
        private const string DefaultTagCoilgunPrefix = "CG";
        private const string DefaultTagMissilePrefix = "Missile";

        private readonly IUnitModel _launchingUnit;
        private readonly IUnitModel _targetUnit;
        private readonly ICurrentTurnModel _currentTurn;
        private readonly IWeaponSelectionModel _weaponSelectionModel;

        private readonly IAvidCalculator _avidCalculator;
        private readonly IHexGridCalculator _hexGridCalculator;
        private readonly IHexVectorUtility _hexVectorUtility;
        private readonly IFiringSolutionCalculator _firingSolutionCalculator;
        private readonly IShellstarBuilder _shellstarBuilder;

        public LaunchBoardModel(IUnitModel launchingUnit, IUnitModel targetUnit, ICurrentTurnModel currentTurn)
        {
            if (launchingUnit == null)
            {
                throw new ArgumentNullException("launchingUnit");
            }

            if (targetUnit == null)
            {
                throw new ArgumentNullException("targetUnit");
            }

            _avidCalculator = ServiceFactory.Library.AvidCalculator;
            _hexGridCalculator = ServiceFactory.Library.HexGridCalculator;
            _hexVectorUtility = ServiceFactory.Library.HexVectorUtility;
            _firingSolutionCalculator = ServiceFactory.Library.FiringSolutionCalculator;
            _shellstarBuilder = ServiceFactory.Library.ShellstarBuilder;
            _weaponSelectionModel = new WeaponSelectionModel();

            _launchingUnit = launchingUnit;
            _targetUnit = targetUnit;
            _currentTurn = currentTurn;
        }

        public event Action VectorsUpdated;
        public event Action FiringSolutionUpdated;
        public event Action BearingVerificationRequired;

        public IUnitModel LaunchingUnit
        {
            get { return _launchingUnit; }
        }

        public IUnitModel Target
        {
            get { return _targetUnit; }
        }

        public TurnData LaunchingSegment { get; private set; }

        public AvidVector TargetDistance { get; private set; }
        public AvidVector CrossingVector { get; private set; }

        public HexGridCoordinate LaunchingUnitPosition { get; private set; }
        public HexGridCoordinate TargetUnitPosition { get; private set; }
        public RawHexVector LaunchingUnitVelocity { get; private set; }
        public RawHexVector TargetUnitVelocity { get; private set; }
        public FiringSolution FiringSolution { get; private set; }
        public MissileAccelerationData AccelerationData { get; private set; }
        public IShellstarModel Shellstar { get; private set; }
        public AvidWindow[] LaunchWindows { get; private set; }

        public int CourseOffset { get; set; }

        public IWeaponSelectionModel WeaponSelection
        {
            get { return _weaponSelectionModel; }
        }

        public void UpdatePlatforms()
        {
            // We need to duplicate everything to control updates of the launch data manually.
            LaunchingSegment = new TurnData(_currentTurn.CurrentTurn.Turn, _currentTurn.CurrentTurn.Impulse);

            LaunchingUnitPosition = _hexVectorUtility.CloneHexVector<HexGridCoordinate>(_launchingUnit.Position);
            TargetUnitPosition = _hexVectorUtility.CloneHexVector<HexGridCoordinate>(_targetUnit.Position);

            LaunchingUnitVelocity = _hexVectorUtility.CloneHexVector<RawHexVector>(_launchingUnit.Vectors);
            TargetUnitVelocity = _hexVectorUtility.CloneHexVector<RawHexVector>(_targetUnit.Vectors);

            CrossingVector = _hexGridCalculator.GetCrossingVector(LaunchingUnitVelocity, TargetUnitVelocity);

            TargetDistance = _hexGridCalculator.GetDistance(LaunchingUnitPosition, TargetUnitPosition);
            CourseOffset = 0;
            if (AvidVector.IsNullOrZero(TargetDistance))
            {
                TargetDistance = _hexGridCalculator.GetVectorFromBacktracking(LaunchingUnitVelocity, TargetUnitVelocity);
                OnVerificationRequired();
            }
            else
            {
                SubmitBearingVerificationResult(TargetDistance);
            }
        }

        public void SubmitBearingVerificationResult(AvidWindow verificationResult)
        {
            if (verificationResult == null)
            {
                OnVerificationRequired();
                return;
            }

            var vectorResult = verificationResult as AvidVector;

            int magnitude = vectorResult != null ? vectorResult.Magnitude : 0;

            TargetDistance = new AvidVector(verificationResult.Direction, verificationResult.Ring, verificationResult.AbovePlane, magnitude);
            CourseOffset = _avidCalculator.GetCourseOffset(TargetDistance, CrossingVector);

            OnVectorsUpdated();
            UpdateFiringSolution(null, null, null);
        }

        public void CalculateFiringSolution()
        {
            var firingSolution = _firingSolutionCalculator.CalculateSolution(
                CourseOffset,
                CrossingVector.Magnitude,
                WeaponSelection.MuzzleVelocityMultiplyer,
                WeaponSelection.Acceleration);

            MissileAccelerationData missileAccelerationData = null;

            if (WeaponSelection.IsMissile && firingSolution.AimAdjustment != AimAdjustment.NoShot)
            {
                missileAccelerationData = _firingSolutionCalculator.CalculateMissileAcceleration(
                    TargetDistance.Magnitude,
                    WeaponSelection.MuzzleVelocityMultiplyer,
                    WeaponSelection.Acceleration,
                    firingSolution.AimAdjustment,
                    firingSolution.RoC);
            }

            IShellstarModel shellstar = BuildShellstar(firingSolution, missileAccelerationData);

            UpdateFiringSolution(firingSolution, missileAccelerationData, shellstar);
        }

        private void UpdateFiringSolution(FiringSolution firingSolution, MissileAccelerationData missileAccelerationData, IShellstarModel shellstar)
        {
            FiringSolution = firingSolution;
            AccelerationData = missileAccelerationData;
            Shellstar = shellstar;
            LaunchWindows = GetAvailableLaunchWindows();
            OnFiringSolutionUpdated();
        }

        private IShellstarModel BuildShellstar(FiringSolution firingSolution, MissileAccelerationData accelerationData)
        {
            if (firingSolution == null || firingSolution.AimAdjustment == AimAdjustment.NoShot ||
                (accelerationData != null && accelerationData.ValidLaunch == false))
            {
                return null;
            }

            var shellstar = _shellstarBuilder.BuildShellstarInfo(TargetDistance.Magnitude, LaunchingSegment, firingSolution, accelerationData);
            var result = new ShellstarModel(shellstar, LaunchingSegment)
            {
                Tag = GetDefaultTag()
            };

            return result;
        }

        private string GetDefaultTag()
        {
            return string.Format(DefaultTagFormat,
                                 WeaponSelection.IsMissile ? DefaultTagMissilePrefix : DefaultTagCoilgunPrefix,
                                 LaunchingSegment,
                                 _launchingUnit.Name);
        }

        private AvidWindow[] GetAvailableLaunchWindows()
        {
            if (FiringSolution == null)
            {
                return new AvidWindow[0];
            }

            return _avidCalculator.GetPossibleLaunchWindows((int)FiringSolution.AimAdjustment, TargetDistance, CrossingVector);
        }

        private void OnVectorsUpdated()
        {
            Action handler = VectorsUpdated;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnFiringSolutionUpdated()
        {
            Action handler = FiringSolutionUpdated;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnVerificationRequired()
        {
            var handler = BearingVerificationRequired;
            if (handler != null)
            {
                handler();
            }
        }
    }
}
