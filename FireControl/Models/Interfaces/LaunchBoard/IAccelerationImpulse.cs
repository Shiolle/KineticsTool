namespace FireControl.Models.Interfaces.LaunchBoard
{
    internal interface IAccelerationImpulse
    {
        float Range { get; }

        float PositionAdjustment { get; }

        float RoC { get; }
    }
}
