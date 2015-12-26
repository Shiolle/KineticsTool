using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Kinetics.Core.Data.Avid;
using Microsoft.Practices.ObjectBuilder2;

namespace FireControl.ViewModels.AvidElementBoard
{
    internal class AvidElementBoardViewModel : ViewModelBase, IAvidElementBoard, IMarkArranger
    {
        private const int MaxVisibleMarksPerCell = 4;
        private readonly ObservableCollection<AvidMarkViewModel> _marks;

        public AvidElementBoardViewModel()
        {
            _marks = new ObservableCollection<AvidMarkViewModel>();
        }

        public void AddMarks(int categoryId, string text, bool visible, bool underlined, IEnumerable<AvidWindow> positions)
        {
            var newMarks = positions.Select(wnd => new AvidMarkViewModel(this, text, visible, underlined, categoryId, -1, wnd));

            newMarks.ForEach(mk => _marks.Add(mk));

            RecalculateSharing();
        }

        public IAvidMark AddMark(int categoryId, string text, bool visible, bool underlined, AvidWindow position)
        {
            var newMark = new AvidMarkViewModel(this, text, visible, underlined, categoryId, -1, position);
            _marks.Add(newMark);
            RecalculateSharing();
            return newMark;
        }

        public void WipeCategory(int categoryId)
        {
            WipeMarksByCategory(categoryId);
            RecalculateSharing();
        }

        public void WipeAllMarks()
        {
            _marks.Clear();
        }

        private void WipeMarksByCategory(int categoryId)
        {
            var marksOfCategory = _marks.Where(mk => mk.CategoryId == categoryId).ToArray();
            marksOfCategory.ForEach(mk => _marks.Remove(mk));
        }

        public void RecalculateSharing()
        {
            var marksByWindow = _marks.GroupBy(mrk => GetWindowCode(mrk.Window), mrk => mrk);
            foreach (var markWindowPair in marksByWindow)
            {
                var marks = markWindowPair.ToList();
                if (marks.Count > 1)
                {
                    marks.Sort((mrk1, mrk2) => mrk1.CategoryId.CompareTo(mrk2.CategoryId));
                    for (int i = 0; i < marks.Count; i++)
                    {
                        marks[i].UpdateSharing(i, i >= MaxVisibleMarksPerCell);
                    }
                }
                else
                {
                    marks.Single().UpdateSharing(-1, false);
                }
            }
            RefreshMarks();
        }

        public ObservableCollection<AvidMarkViewModel> Marks
        {
            get { return _marks; }
        }

        /// <summary>
        /// This method is needed because we have to ignore position relative to the plane of AVID when calculating shering.
        /// Ex. F+ and F- are still in the same window on AVID.
        /// </summary>
        private int GetWindowCode(AvidWindow window)
        {
            return 10 * (byte)window.Direction + (byte)window.Ring;
        }

        private void RefreshMarks()
        {
            CollectionViewSource.GetDefaultView(Marks).Refresh();
        }
    }
}
