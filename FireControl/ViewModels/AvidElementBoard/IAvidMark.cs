using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.AvidElementBoard
{
    /// <summary>
    /// Provides aan interface to manipulate individual marks.
    /// </summary>
    internal interface IAvidMark
    {
        /// <summary>
        /// Gets category ID.
        /// </summary>
        int CategoryId { get; }

        /// <summary>
        /// Gets or sets the text that will be displayd.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets mark's visibility.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets underlined status.
        /// </summary>
        bool Underlined { get; set; }

        /// <summary>
        /// Gets or sets postion on the AVID.
        /// </summary>
        AvidWindow Window { get; set; }
    }
}
