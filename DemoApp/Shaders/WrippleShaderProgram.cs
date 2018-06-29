using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL;
using System.Windows;
using System.IO;
using System.Windows.Media;

namespace DemoApp
{
    /// <summary> A shader program that wripples vertices </summary>
    public class WrippleShaderProgram : ShaderProgram
    {
        const float PI2 = (float)Math.PI * 2;

        #region Frequency
        [ShaderProperty("frequency")]
        public float Frequency
        {
            get { return (float)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }
        public static readonly DependencyProperty FrequencyProperty =
            DependencyProperty.Register("Frequency", typeof(float), typeof(WrippleShaderProgram), new UIPropertyMetadata(1.0f));
        #endregion

        #region Speed
        public float Speed
        {
            get { return (float)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }
        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.Register("Speed", typeof(float), typeof(WrippleShaderProgram), new UIPropertyMetadata(0.05f));
        #endregion

        #region Scale
        [ShaderProperty("wrippleScale")]
        public float Scale
        {
            get { return (float)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(float), typeof(WrippleShaderProgram), new UIPropertyMetadata(0.3f));
        #endregion

        #region Phase
        [ShaderProperty("phase")]
        public float Phase
        {
            get { return (float)GetValue(PhaseProperty); }
            set { SetValue(PhaseProperty, value); }
        }
        public static readonly DependencyProperty PhaseProperty =
            DependencyProperty.Register("Phase", typeof(float), typeof(WrippleShaderProgram), new UIPropertyMetadata(0f));
        #endregion

        #region Color
        [ShaderProperty("colorX")]
        public Color ColorX
        {
            get { return (Color)GetValue(ColorXProperty); }
            set { SetValue(ColorXProperty, value); }
        }
        public static readonly DependencyProperty ColorXProperty =
            DependencyProperty.Register("ColorX", typeof(Color), typeof(WrippleShaderProgram), new UIPropertyMetadata(Colors.Red));

        [ShaderProperty("colorY")]
        public Color ColorY
        {
            get { return (Color)GetValue(ColorYProperty); }
            set { SetValue(ColorYProperty, value); }
        }
        public static readonly DependencyProperty ColorYProperty =
            DependencyProperty.Register("ColorY", typeof(Color), typeof(WrippleShaderProgram), new UIPropertyMetadata(Colors.Lime));

        [ShaderProperty("colorZ")]
        public Color ColorZ
        {
            get { return (Color)GetValue(ColorZProperty); }
            set { SetValue(ColorZProperty, value); }
        }
        public static readonly DependencyProperty ColorZProperty =
            DependencyProperty.Register("ColorZ", typeof(Color), typeof(WrippleShaderProgram), new UIPropertyMetadata(Colors.Blue));
        #endregion

        public WrippleShaderProgram()
        {
            Initialize();
        }

        public override void Activate()
        {
            // update the phase
            Phase += Speed;
            if (Phase > PI2)
                Phase = Phase % PI2;

            base.Activate();
        }

        public void Initialize()
        {
            var file = new FileInfo(@"Shaders\Wripple_VS.glsl");
            this.VertexShader = new VertexShader(file);
        }
    }
}
