using System;
using System.Collections.Generic;
using System.Linq;
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
using System.ComponentModel;

namespace WPFOpenTK
{
    /// <summary> Implementation of the slider from Expression Blend </summary>
    /// <remarks> 
    /// Original control from http://dotwaywpf.codeplex.com 
    /// This control still needs work
    /// </remarks>
    [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_InteractionShape", Type = typeof(Shape))]
    public class EditSlider : Control
    {
        #region Fields

        private enum PointerState
        {
            Idle = 0,
            Moved = 1,
            Pressed = 2
        };

        private PointerState pointerState = PointerState.Idle;
        private TextBox textBox = null;
        private Point lastPosition = new Point();
        private Shape interactionControl = null;
        private Vector offsetUp = new Vector(1, 0);
        private Vector offsetDown = new Vector(-1, 0);

        #endregion Fields
        #region Constructos

        public EditSlider()
        {
            this.MinHeight = 0;
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            this.PreviewKeyUp += new KeyEventHandler(OnPreviewKeyUp);            
        }             

        static EditSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditSlider), new FrameworkPropertyMetadata(typeof(EditSlider)));
        }

        #endregion Constructors
        #region Properties

        public static readonly DependencyProperty IsValueEditingProperty = DependencyProperty.Register("IsValueEditing", typeof(bool), typeof(EditSlider), new UIPropertyMetadata(false));
        public bool IsValueEditing
        {
            get { return (bool)GetValue(IsValueEditingProperty); }
            set { SetValue(IsValueEditingProperty, value); }
        }


        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(EditSlider), new UIPropertyMetadata(Orientation.Horizontal, new PropertyChangedCallback(OnOrientationChanged)));
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #region OnOrientationChanged

        private static void OnOrientationChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                control.OnOrientationChanged((Orientation)e.OldValue, (Orientation)e.NewValue);
            }
        }

        protected virtual void OnOrientationChanged(Orientation oldValue, Orientation newValue)
        {
            SetPosition(Value);
        }

        #endregion OnOrientationChanged                
        
         
        public static readonly DependencyProperty ValuePositionProperty = DependencyProperty.Register("ValuePosition", typeof(double), typeof(EditSlider), new UIPropertyMetadata(5.0));
        public double ValuePosition
        {
            get { return (double)GetValue(ValuePositionProperty); }
            private set { SetValue(ValuePositionProperty, value); }
        }


        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(EditSlider), new UIPropertyMetadata(5.0, new PropertyChangedCallback(OnValueChanged)));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #region OnValueChanged
        public event DependencyPropertyChangedEventHandler ValueChanged;
        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                control.OnValueChanged((double)e.OldValue, (double)e.NewValue);
            }
            DependencyPropertyChangedEventHandler temp = control.ValueChanged;
            if (temp != null)
            {
                temp(o, e);
            }
        }

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            CoerceValue(MinimumProperty);
            CoerceValue(MaximumProperty);

            if (newValue < Minimum)
            {
                Value = Minimum;
                newValue = Minimum;
            }
            if (newValue > Maximum)
            {
                Value = Maximum;
                newValue = Maximum;
            }

            SetPosition(newValue);            
        }

        #endregion OnValueChanged               


        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(double), typeof(EditSlider), new UIPropertyMetadata(1.0));
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }


        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(EditSlider), new UIPropertyMetadata(0.0, new PropertyChangedCallback(OnMinimumChanged), new CoerceValueCallback(OnCoerceMinimum)));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #region OnMinimumChanged

        private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                control.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
            }
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue)
        {
            CoerceValue(MaximumProperty);
            CheckValue();
        }

        #endregion OnMinimumChanged
        #region OnCoerceMinimum

        private static object OnCoerceMinimum(DependencyObject o, object value)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                return control.OnCoerceMinimum((double)value);
            }
            else
            {
                return value;
            }
        }

        protected virtual double OnCoerceMinimum(double value)
        {
            if (value > Maximum)
            {
                value = Maximum;
            }

            return value;
        }

        #endregion OnCoerceMinimum


        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(EditSlider), new UIPropertyMetadata(10.0, new PropertyChangedCallback(OnMaximumChanged), new CoerceValueCallback(OnCoerceMaximum)));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #region OnMaximumChanged

        private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                control.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
            }
        }

        protected virtual void OnMaximumChanged(double oldValue, double newValue)
        {
            CoerceValue(MinimumProperty);
            CheckValue();            
        }

        #endregion OnMaximumChanged
        #region OnCoerceMaximum

        private static object OnCoerceMaximum(DependencyObject o, object value)
        {
            EditSlider control = o as EditSlider;
            if (control != null)
            {
                return control.OnCoerceMaximum((double)value);
            }
            else
            {
                return value;
            }
        }

        protected virtual double OnCoerceMaximum(double value)
        {
            if (value < Minimum)
            {
                value = Minimum;
            }

            return value;
        }

        #endregion OnCoerceMaximum


        #region DecimalCount
        [Description("The number of digits past the decimal point.")]
        public int DecimalCount
        {
            get { return (int)GetValue(DecimalCountProperty); }
            set { SetValue(DecimalCountProperty, value); }
        }
        public static readonly DependencyProperty DecimalCountProperty =
            DependencyProperty.Register("DecimalCount", typeof(int), typeof(EditSlider), new UIPropertyMetadata(2)); 
        #endregion

        #region ShowText
        [Description("Show the number or not.")]
        public Visibility ShowText
        {
            get { return (Visibility)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }
        public static readonly DependencyProperty ShowTextProperty =
            DependencyProperty.Register("ShowText", typeof(Visibility), typeof(EditSlider), new UIPropertyMetadata(Visibility.Visible));
        #endregion

        private double StepDistance 
        {
            get 
            { 
                var stepCount = (Maximum - Minimum) / Step;
                return ActualHeight / stepCount; 
            }
        }
        #endregion Properties

        public override void OnApplyTemplate()
        {
            interactionControl = this.GetTemplateChild("PART_InteractionShape") as Shape;
            if (interactionControl == null)
            {
                throw new NullReferenceException("Could not find templated part: PART_InteractionShape.");
            }
            interactionControl.MouseLeftButtonDown += new MouseButtonEventHandler(OnMouseLeftButtonDown);
            interactionControl.MouseLeftButtonUp += new MouseButtonEventHandler(OnMouseLeftButtonUp);
            interactionControl.MouseMove += new MouseEventHandler(OnMouseMove);
            interactionControl.SizeChanged += new SizeChangedEventHandler(OnSizeChanged);            

            textBox = this.GetTemplateChild("PART_TextBox") as TextBox;
            if (textBox == null)
            {
                throw new NullReferenceException("Could not find templated part: PART_TextBox.");
            }
            textBox.KeyUp += new KeyEventHandler(OnTextBoxKeyUp);
            textBox.GotFocus += new RoutedEventHandler(OnTextBoxGotFocus);            
            textBox.LostFocus += new RoutedEventHandler(OnTextBoxLostFocus);            
            textBox.TextChanged += new TextChangedEventHandler(OnTextBoxTextChanged);

            base.OnApplyTemplate();
        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            double testInput = 0.0;
            if (!double.TryParse(textBox.Text, out testInput))
            {
                textBox.Text = "0";
            }
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            IsValueEditing = false;
        }        

        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            textBox.SelectAll();
        }

        private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            Key pressedKey = e.Key;            
            if (pressedKey == Key.Enter)
            {
                this.Focus();                
                e.Handled = true;
            }            
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Key pressedKey = e.Key;
            if (pressedKey == Key.Enter)
            {
                if (ShowText == Visibility.Visible)
                    IsValueEditing = true;
            }            
        }

        private void OnPreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key pressedKey = e.Key;            
            if (pressedKey == Key.Up || pressedKey == Key.Right)
            {
                OffsetValue(offsetUp);
            }
            else if (pressedKey == Key.Down || pressedKey == Key.Left)
            {
                OffsetValue(offsetDown);
            }
        }  

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                pointerState = PointerState.Moved;

                Point currentPosition = e.GetPosition(this);
                Vector translate = new Vector(currentPosition.X - lastPosition.X, -(currentPosition.Y - lastPosition.Y));
                if (Math.Abs(translate.X + translate.Y) > StepDistance)
                {
                    OffsetValue(translate);
                    lastPosition = currentPosition;
                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);

            if (pointerState == PointerState.Pressed)
            {
                if (ShowText == Visibility.Visible)
                    IsValueEditing = true;
            }

            pointerState = PointerState.Idle;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pointerState = PointerState.Pressed;
            lastPosition = e.GetPosition(this);            

            interactionControl.CaptureMouse();
        }        

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPosition(Value);
            this.UpdateLayout();
        }

        private void OffsetValue(Vector offset)
        {
            Value += Math.Truncate((offset.X + offset.Y) / StepDistance) * Step;
        }

        private void SetPosition(double value)
        {
            if (interactionControl != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    ValuePosition = interactionControl.ActualWidth * ((value - Minimum) / (Maximum - Minimum));
                }
                else
                {
                    ValuePosition = interactionControl.ActualHeight * ((value - Minimum) / (Maximum - Minimum));
                }                
            }                      
        }

        //private void CheckMaximum()
        //{
        //    if (Maximum < Minimum)
        //    {

        //    }
        //}

        //private void CheckMinimum()
        //{
        //    if (Minimum > Maximum)
        //    {
        //        Minimum = Maximum;
        //    }            
        //}

        private void CheckValue()
        {
            if (Value < Minimum)
            {
                Value = Minimum;                
            }
            if (Value > Maximum)
            {
                Value = Maximum;                
            }
        }
    }
}
