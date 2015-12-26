using System;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core.Data;
using Kinetics.Core.Data.Avid;
using Kinetics.Core.Data.HexGrid;
using Kinetics.Core.Data.FiringSolution;
using Kinetics.Core.Data.HexVectors;

namespace FireControl.Models.Interfaces.LaunchBoard
{
    /// <summary>
    /// The model of AV:T launch board.
    /// </summary>
    internal interface ILaunchBoardModel
    {
        /// <summary>
        /// Invoked when positions, velocities, Target distance and crossing vectors are updated.
        /// </summary>
        event Action VectorsUpdated;

        /// <summary>
        /// Invoked when it is impossible to determine target bearing with absolute certainty. Usually means that attacker and target are in the same hex.
        /// At this time, TargetDistance is a bearing calculated from backtracking, or, when relative velocity is also zero, window A.
        /// </summary>
        event Action BearingVerificationRequired;

        /// <summary>
        /// Invoked when fire solution is recalculated.
        /// </summary>
        event Action FiringSolutionUpdated;

        /// <summary>
        /// Gets the launching unit.
        /// </summary>
        IUnitModel LaunchingUnit { get; }

        /// <summary>
        /// Gets target unit.
        /// </summary>
        IUnitModel Target { get; }

        /// <summary>
        /// Gets shellstar for current firing solution.
        /// </summary>
        IShellstarModel Shellstar { get; }

        /// <summary>
        /// Gets or sets course offset.
        /// </summary>
        int CourseOffset { get; set; }

        /// <summary>
        /// Gets turn and segment of launch. May differ from current.
        /// </summary>
        TurnData LaunchingSegment { get; }

        /// <summary>
        /// Gets target distance and direction as seen from th launching platform.
        /// </summary>
        AvidVector TargetDistance { get; }

        /// <summary>
        /// Gets crossing vector.
        /// </summary>
        AvidVector CrossingVector { get; }

        /// <summary>
        /// Gets launching unit position.
        /// </summary>
        HexGridCoordinate LaunchingUnitPosition { get; }

        /// <summary>
        /// Gets target unit position.
        /// </summary>
        HexGridCoordinate TargetUnitPosition { get; }

        /// <summary>
        /// Gets launching unit velocity.
        /// </summary>
        RawHexVector LaunchingUnitVelocity { get; }

        /// <summary>
        /// Gets target velocity.
        /// </summary>
        RawHexVector TargetUnitVelocity { get; }

        /// <summary>
        /// Gets firing solution.
        /// </summary>
        FiringSolution FiringSolution { get; }

        /// <summary>
        /// Gets missile acceleration data.
        /// </summary>
        MissileAccelerationData AccelerationData { get; }

        /// <summary>
        /// Gets model for weapon selection.
        /// </summary>
        IWeaponSelectionModel WeaponSelection { get; }

        /// <summary>
        /// Gets the list of possible launch windows.
        /// </summary>
        AvidWindow[] LaunchWindows { get; }

        /// <summary>
        /// Updates positions, velocities, Target distance and crossing vectors.
        /// </summary>
        void UpdatePlatforms();

        /// <summary>
        /// Process the result of bearing verification.
        /// </summary>
        /// <param name="verificationResult">Verification result</param>
        void SubmitBearingVerificationResult(AvidWindow verificationResult);

        /// <summary>
        /// Calculates firing solution.
        /// </summary>
        void CalculateFiringSolution();
    }
}
