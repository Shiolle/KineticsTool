using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireControl.ViewModels.AvidElementBoard
{
    /// <summary>
    /// Represents AVID element board as seen by a mark.
    /// </summary>
    internal interface IMarkArranger
    {
        /// <summary>
        /// Recalculates sharing positions and updates the board.
        /// </summary>
        void RecalculateSharing();
    }
}
