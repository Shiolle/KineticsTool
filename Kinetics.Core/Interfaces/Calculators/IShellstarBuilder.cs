using Kinetics.Core.Data;
using Kinetics.Core.Data.FiringSolution;

namespace Kinetics.Core.Interfaces.Calculators
{
    public interface IShellstarBuilder
    {
        ShellstarInfo BuildShellstarInfo(int targetDistance, TurnData startingImpulse, FiringSolution firingSolution, MissileAccelerationData missileAcceleration);
    }
}
