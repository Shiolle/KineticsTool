using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireControl.Models.Interfaces.ShellStars;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.ViewModels.DataContexts
{
    internal class CounterfireDataContext
    {
        public IShellstarModel IncomingShellstar { get; set; }
        public IUnitModel Target { get; set; }
        public IUnitListModel UnitList { get; set; }
    }
}
