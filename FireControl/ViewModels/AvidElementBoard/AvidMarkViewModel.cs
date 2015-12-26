using Kinetics.Core.Data.Avid;

namespace FireControl.ViewModels.AvidElementBoard
{
    internal class AvidMarkViewModel : ViewModelBase, IAvidMark
    {
        private readonly IMarkArranger _markArranger;

        private string _text;
        private bool _visible;
        private bool _underlined;
        private int _categoryId;
        private bool _isOverspill;
        private AvidWindow _window;

        public AvidMarkViewModel()
        {
            _window = new AvidWindow();
        }

        public AvidMarkViewModel(IMarkArranger markArranger, string text, bool visible, bool underlined, int categoryId, int sharingPosition, AvidWindow window)
        {
            _markArranger = markArranger;
            Text = text;
            Underlined = underlined;
            Visible = visible;
            CategoryId = categoryId;
            SharingPosition = sharingPosition;
            Window = window;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == null || !_text.Equals(value))
                {
                    _text = value;
                    OnPropertyChanged(Properties.Text);
                }
            }
        }

        public bool Underlined
        {
            get { return _underlined; }
            set
            {
                if (_underlined != value)
                {
                    _underlined = value;
                    OnPropertyChanged(Properties.Underlined);
                }
            }
        }

        public bool Visible
        {
            get { return _visible && !_isOverspill; }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnPropertyChanged(Properties.Visible);
                }
            }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    OnPropertyChanged(Properties.CategoryId);
                }
            }
        }

        public int SharingPosition { get; private set; }

        public AvidWindow Window
        {
            get { return _window; }
            set
            {
                _window = value;
                _markArranger.RecalculateSharing();
                OnPropertyChanged(Properties.Window);
            }
        }

        public void UpdateSharing(int sharingPosition, bool isOverspill)
        {
            SharingPosition = sharingPosition;
            _isOverspill = isOverspill;
            OnPropertyChanged(Properties.Window);
        }

        private static class Properties
        {
            public const string Text = "Text";
            public const string Underlined = "Underlined";
            public const string Visible = "Visible";
            public const string CategoryId = "CategoryId";
            //public const string SharingPosition = "SharingPosition";
            public const string Window = "Window";
        }
    }
}
