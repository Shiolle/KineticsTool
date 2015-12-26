namespace FireControl.Commands
{
    /// <summary>
    /// Moves a unit in a single direction. Unit controlled through view model; direction controlled by markup.
    /// </summary>
    internal class MoveUnitCommand : SimpleCommand
    {
        public string Direction { get; set; }

        override public void Execute(object parameter)
        {
            InvokeMethod(new object[]{ Direction });
        }
    }
}
