using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.Avid
{
    internal class AvidVectorViewModel : ViewModelBase
    {
        public void UpdateVector(AvidVector vector, bool isVisible)
        {
            IsVisible = !AvidWindow.IsNullOrZero(vector) && isVisible;
            Magnitude = vector != null ? vector.Magnitude : 0;
            Window = vector;

            OnPropertyChanged(Properties.IsVisible);
            OnPropertyChanged(Properties.Magnitude);
            OnPropertyChanged(Properties.Window);
        }

        public bool IsVisible { get; private set; }

        public int Magnitude { get; private set; }

        public AvidWindow Window { get; private set; }

        private static class Properties
        {
            public const string IsVisible = "Visible";
            public const string Magnitude = "Magnitude";
            public const string Window = "Window";
        }
    }
}
