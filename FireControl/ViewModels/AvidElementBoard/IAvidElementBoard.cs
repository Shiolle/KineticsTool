using System.Collections.Generic;
using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.AvidElementBoard
{
    /// <summary>
    /// This is the "end user" interface for the element board. It allows to arrange items on the board.
    /// </summary>
    internal interface IAvidElementBoard
    {
        /// <summary>
        /// Adds a single marke to the board.
        /// </summary>
        /// <param name="categoryId">Mark category. Used for manipulating marks in groups and as arrangemnt priority rank.</param>
        /// <param name="text">Mark text.</param>
        /// <param name="visible">Mark visibility.</param>
        /// <param name="underlined">Specifies whther the mark should be underlined.</param>
        /// <param name="position">Mark position on AVID.</param>
        /// <returns>Mark interface.</returns>
        IAvidMark AddMark(int categoryId, string text, bool visible, bool underlined, AvidWindow position);

        /// <summary>
        /// Adds a group of marks with one content, but in different positions.
        /// </summary>
        /// <param name="categoryId">Mark category. Used for manipulating marks in groups and as arrangemnt priority rank.</param>
        /// <param name="text">Mark text.</param>
        /// <param name="visible">Mark visibility.</param>
        /// <param name="underlined">Specifies whther the mark should be underlined.</param>
        /// <param name="positions">The list of positions. Determines how many marks will be created.</param>
        void AddMarks(int categoryId, string text, bool visible, bool underlined, IEnumerable<AvidWindow> positions);

        /// <summary>
        /// Deletes all amrks in a specific category.
        /// </summary>
        /// <param name="categoryId">Category ID.</param>
        void WipeCategory(int categoryId);

        /// <summary>
        /// Deletes all marks.
        /// </summary>
        void WipeAllMarks();
    }
}
