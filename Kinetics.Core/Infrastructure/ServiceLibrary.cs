using Kinetics.Core.Interfaces.Calculators;
using Kinetics.Core.Interfaces.Infrastructure;
using Kinetics.Core.Interfaces.RefData;

namespace Kinetics.Core.Infrastructure
{
    /// <summary>
    /// This class is using dependency injection to resolve all calculators and reference table classes at once,
    /// so that Container resolved could be calledonly once in the composition root.
    /// </summary>
    internal class ServiceLibrary : IServiceLibrary
    {
        private readonly IAvidCalculator _avidCalculator;
        private readonly IAvidProjectionCalculator _avidProjectionCalculator;
        private readonly IHexGridCalculator _hexGridCalculator;
        private readonly IHexVectorUtility _hexVectorUtility;
        private readonly IFiringSolutionCalculator _firingSolutionCalculator;
        private readonly IShellstarBuilder _shellstarBuilder;

        private readonly IRangeAltitudeTable _rangeAltitudeTable;
        private readonly IShotGeometryTable _shotGeometryTable;
        private readonly IMissilePositionAdjustmentTable _missilePositionAdjustmentTable;
        private readonly IProjectileDamageTable _projectileDamageTable;

        public ServiceLibrary(IAvidCalculator avidCalculator,
                           IAvidProjectionCalculator avidProjectionCalculator,
                           IHexGridCalculator hexGridCalculator,
                           IHexVectorUtility hexVectorUtility,
                           IFiringSolutionCalculator firingSolutionCalculator,
                           IShellstarBuilder shellstarBuilder,
                           IRangeAltitudeTable rangeAltitudeTable,
                           IShotGeometryTable shotGeometryTable,
                           IMissilePositionAdjustmentTable missilePositionAdjustmentTable,
                           IProjectileDamageTable projectileDamageTable)
        {
            _avidCalculator = avidCalculator;
            _avidProjectionCalculator = avidProjectionCalculator;
            _hexGridCalculator = hexGridCalculator;
            _hexVectorUtility = hexVectorUtility;
            _firingSolutionCalculator = firingSolutionCalculator;
            _shellstarBuilder = shellstarBuilder;

            _rangeAltitudeTable = rangeAltitudeTable;
            _shotGeometryTable = shotGeometryTable;
            _missilePositionAdjustmentTable = missilePositionAdjustmentTable;
            _projectileDamageTable = projectileDamageTable;
        }

        #region Calculators

        public IAvidCalculator AvidCalculator
        {
            get { return _avidCalculator; }
        }

        public IAvidProjectionCalculator AvidProjectionCalculator
        {
            get { return _avidProjectionCalculator; }
        }

        public IFiringSolutionCalculator FiringSolutionCalculator
        {
            get { return _firingSolutionCalculator; }
        }

        public IHexGridCalculator HexGridCalculator
        {
            get { return _hexGridCalculator; }
        }

        public IHexVectorUtility HexVectorUtility
        {
            get { return _hexVectorUtility; }
        }

        public IShellstarBuilder ShellstarBuilder
        {
            get { return _shellstarBuilder; }
        }

        #endregion

        #region ReferenceLibraries

        public IRangeAltitudeTable RangeAltitudeTable
        {
            get { return _rangeAltitudeTable; }
        }

        public IShotGeometryTable ShotGeometryTable
        {
            get { return _shotGeometryTable; }
        }

        public IMissilePositionAdjustmentTable MissilePositionAdjustmentTable
        {
            get { return _missilePositionAdjustmentTable; }
        }

        public IProjectileDamageTable ProjectileDamageTable
        {
            get { return _projectileDamageTable; }
        }

        #endregion
    }
}