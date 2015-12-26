using FireControl.Models.Interfaces.UnitControl;

namespace FireControl.Commands
{
    internal class SelectUnitCommand : SimpleCommand
    {
        public SelectionSlot Slot { get; set; }

        public override void Execute(object parameter)
        {
            base.Execute(Slot);
        }
    }
}
