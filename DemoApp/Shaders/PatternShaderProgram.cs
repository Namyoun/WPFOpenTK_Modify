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
    /// <summary> Just makes a pretty pattern in the fragment shader </summary>
    public class PatternShaderProgram : ShaderProgram
    {
        const float PI2 = (float)Math.PI * 2;

        #region Pattern
        /// <summary> The pattern to use </summary>
        [ShaderProperty("pattern")]
        public int Pattern
        {
            get { return (int)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(int), typeof(PatternShaderProgram), new UIPropertyMetadata(3)); 
        #endregion

        public PatternShaderProgram()
        {
            Initialize();
        }

        public override void Activate()
        {
            base.Activate();
        }

        public void Initialize()
        {
            var file = new FileInfo(@"Shaders\Simple_VS.glsl");
            this.VertexShader = new VertexShader(file);
            file = new FileInfo(@"Shaders\Pattern_FS.glsl");
            this.FragmentShader = new FragmentShader(file);
        }
    }
}
