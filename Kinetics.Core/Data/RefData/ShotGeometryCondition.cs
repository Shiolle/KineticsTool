namespace Kinetics.Core.Data.RefData
{
    internal enum ShotGeometryCondition
    {
        None = 0,
        Always = 1,
        L05_10 = 2,  // <0.5:1
        L10_10 = 3,  // <1:1
        L15_10 = 4,  // <1.5:1
        L20_10 = 5,  // <2:1
    }
}
