using Kinetics.Core.Data.FiringSolution;

namespace FireControl.Models.Implementation.ShellStars
{
    /// <summary>
    /// This model is needed only for hiding the last impulse track element that
    /// occupies the 'hit' box on the shellstar.
    /// </summary>
    internal class ImpulseTrackElementModel : ImpulseTrackElement
    {
        public ImpulseTrackElementModel(ImpulseTrackElement source)
        {
            IsVisible = true;
            Range = source.Range;
            IsBurning = source.IsBurning;
            Impulse = source.Impulse;
        }

        public bool IsVisible { get; set; }
    }
}
