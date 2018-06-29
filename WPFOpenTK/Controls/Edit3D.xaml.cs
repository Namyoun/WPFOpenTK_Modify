using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.ComponentModel;

namespace WPFOpenTK
{
	/// <summary>
	/// Interaction logic for Edit3D.xaml
	/// </summary>
	public partial class Edit3D : INotifyPropertyChanged
    {
        #region Caption
        public string Caption
        {
            get { return (string)txtCaption.GetValue(TextBlock.TextProperty); }
            set { txtCaption.SetValue(TextBlock.TextProperty, value); }
        }
        #endregion

        #region Minimum
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Edit3D), new UIPropertyMetadata(-1.0));
        #endregion

        #region Maximum
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Edit3D), new UIPropertyMetadata(1.0));
        #endregion

        #region SmallChange
        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallChangeProperty =
            DependencyProperty.Register("SmallChange", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.01));
        #endregion

        #region LargeChange
        public double LargeChange
        {
            get { return (double)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeChangeProperty =
            DependencyProperty.Register("LargeChange", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.2));
        #endregion

        #region TickFrequency
        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TickFrequency.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TickFrequencyProperty =
            DependencyProperty.Register("TickFrequency", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.2));
        #endregion

        #region DecimalCount
        public int DecimalCount
        {
            get { return (int)GetValue(DecimalCountProperty); }
            set { SetValue(DecimalCountProperty, value); }
        }
        public static readonly DependencyProperty DecimalCountProperty =
            DependencyProperty.Register("DecimalCount", typeof(int), typeof(Edit3D), new UIPropertyMetadata(2));
        #endregion

        #region Value
        public Point3D Value
        {
            get { return (Point3D)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(Point3D), typeof(Edit3D), new UIPropertyMetadata(new Point3D(0, 0, 0), Value_Changed));
        static void Value_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edit3D = sender as Edit3D;
            var newValue = (Point3D)e.NewValue;
            if (edit3D.X != newValue.X)
                edit3D.X = newValue.X;
            if (edit3D.Y != newValue.Y)
                edit3D.Y = newValue.Y;
            if (edit3D.Z != newValue.Z)
                edit3D.Z = newValue.Z;
        }
        #endregion

        #region Individual
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.0, X_Changed));
        static void X_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edit3D = sender as Edit3D;
            var value = edit3D.Value;
            edit3D.Value = new Point3D((double)e.NewValue, value.Y, value.Z);
        }


        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.0, Y_Changed));
        static void Y_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edit3D = sender as Edit3D;
            var value = edit3D.Value;
            edit3D.Value = new Point3D(value.X, (double)e.NewValue, value.Z);
        }


        public double Z
        {
            get { return (double)GetValue(ZProperty); }
            set { SetValue(ZProperty, value); }
        }
        public static readonly DependencyProperty ZProperty =
            DependencyProperty.Register("Z", typeof(double), typeof(Edit3D), new UIPropertyMetadata(0.0, Z_Changed));
        static void Z_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var edit3D = sender as Edit3D;
            var value = edit3D.Value;
            edit3D.Value = new Point3D(value.X, value.Y, (double)e.NewValue);
        }
        #endregion

        #region ResetTimeSpan
        public TimeSpan ResetTimeSpan
        {
            get { return (TimeSpan)GetValue(ResetTimeSpanProperty); }
            set { SetValue(ResetTimeSpanProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ResetTimeSpan.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ResetTimeSpanProperty =
            DependencyProperty.Register("ResetTimeSpan", typeof(TimeSpan), typeof(Edit3D), new UIPropertyMetadata(TimeSpan.FromMilliseconds(2000))); 
        #endregion



        public Edit3D()
		{
			this.InitializeComponent();

            var story = this.Resources["OnbtnResetClick"] as Storyboard;
            story.FillBehavior = FillBehavior.Stop;
            story.Completed += new EventHandler(storyOnbtnResetClick_Completed);
		}

        void storyOnbtnResetClick_Completed(object sender, EventArgs e)
        {
            BeginAnimation(XProperty, null);
            BeginAnimation(YProperty, null);
            BeginAnimation(ZProperty, null);
            SetValue(XProperty, XProperty.DefaultMetadata.DefaultValue);
            SetValue(YProperty, YProperty.DefaultMetadata.DefaultValue);
            SetValue(ZProperty, ZProperty.DefaultMetadata.DefaultValue);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        #endregion

        /// <summary> WPF binding is finiky. Calling this in a timer event </summary>
        public void Update()
        {
            Value = new Point3D(X, Y, Z);
        }
    }
}