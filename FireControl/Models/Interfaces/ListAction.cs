namespace FireControl.Models.Interfaces
{
    /// <summary>
    /// Lists possible operations over items in a list.
    /// It can be applied to units or shellstars or any other list.
    /// </summary>
    internal enum ListAction
    {
        Added = 0,
        Removed = 1,
        Reset = 2
    }
}
