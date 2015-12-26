using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.DataContexts
{
    internal class WindowSelectionContext
    {
        public string Caption { get; set; }

        public string Message { get; set; }

        public AvidWindow InitialWindow { get; set; }

        public bool CanSelectDirection { get; set; }

        public bool CanSelectRing { get; set; }
    }
}
