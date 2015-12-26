using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.ViewModels.DataContexts
{
    internal class LaunchBoardDataContext
    {
        public IUnitModel Attacker { get; set; }

        public IUnitModel Target { get; set; }
    }
}
