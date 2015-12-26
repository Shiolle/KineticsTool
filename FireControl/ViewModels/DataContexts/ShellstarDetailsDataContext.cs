using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.DataContexts
{
    internal class ShellstarDetailsDataContext
    {
        public IUnitModel Target { get; set; }
        public IShellstarModel Shellstar { get; set; }
        public AvidWindow[] LaunchWindows { get; set; }
    }
}
