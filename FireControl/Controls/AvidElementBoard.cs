using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FireControl.Controls
{
    public class AvidElementBoard : ItemsControl
    {
        static AvidElementBoard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AvidElementBoard), new FrameworkPropertyMetadata(typeof(AvidElementBoard)));
        }

        public AvidElementBoard()
            :base()
        {
            //DataContext = this;
        }

        public static readonly DependencyProperty ElementRadiusProperty =
            DependencyProperty.Register("ElementRadius", typeof(double), typeof(AvidElementBoard), new PropertyMetadata(35d));

        public static readonly DependencyProperty RingRadiusProperty =
            DependencyProperty.Register("RingRadius", typeof(double), typeof(AvidElementBoard), new PropertyMetadata(137d));

        public double ElementRadius
        {
            get { return (double)GetValue(ElementRadiusProperty); }
            set { SetValue(ElementRadiusProperty, value); }
        }

        public double RingRadius
        {
            get { return (double)GetValue(RingRadiusProperty); }
            set { SetValue(RingRadiusProperty, value); }
        }

        public Brush ElBkg { get; set; }
    }
}
