using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Interfaces.Infrastructure
{
    /// <summary>
    /// Provides easy access to all services.
    /// </summary>
    public interface IServiceLibrary
    {
        IAvidCalculator AvidCalculator { get; }

        IAvidProjectionCalculator AvidProjectionCalculator { get; }

        IHexGridCalculator HexGridCalculator { get; }

        IHexVectorUtility HexVectorUtility { get; }

        IFiringSolutionCalculator FiringSolutionCalculator { get; }

        IShellstarBuilder ShellstarBuilder { get; }

        IRangeAltitudeTable RangeAltitudeTable { get; }

        IShotGeometryTable ShotGeometryTable { get; }

        IMissilePositionAdjustmentTable MissilePositionAdjustmentTable { get; }

        IProjectileDamageTable ProjectileDamageTable { get; }
    }
}
