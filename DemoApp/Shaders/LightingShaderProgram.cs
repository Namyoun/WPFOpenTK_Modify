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
    /// <summary> Performs fragment lighting </summary>
    public class LightingShaderProgram : ShaderProgram
    {
        const float PI2 = (float)Math.PI * 2;

        #region LightingOffset
        /// <summary> The distance to shift the light </summary>
        [ShaderProperty("LightOffset")]
        public float LightingOffset
        {
            get { return (float)GetValue(LightingOffsetProperty); }
            set { SetValue(LightingOffsetProperty, value); }
        }
        public static readonly DependencyProperty LightingOffsetProperty =
            DependencyProperty.Register("LightingOffset", typeof(float), typeof(LightingShaderProgram), new UIPropertyMetadata(0f)); 
        #endregion

        #region LightingOffsetDelta
        /// <summary> The speed of the light </summary>
        public float LightingOffsetDelta
        {
            get { return (float)GetValue(LightingOffsetDeltaProperty); }
            set { SetValue(LightingOffsetDeltaProperty, value); }
        }
        public static readonly DependencyProperty LightingOffsetDeltaProperty =
            DependencyProperty.Register(
                "LightingOffsetDelta",
                typeof(float),
                typeof(LightingShaderProgram),
                new PropertyMetadata(0.03f));
        #endregion LightingOffsetDelta

        #region UseLighting
        /// <summary> whether to use lighting </summary>
        [ShaderProperty("UseLighting")]
        public bool UseLighting
        {
            get { return (bool)GetValue(UseLightingProperty); }
            set { SetValue(UseLightingProperty, value); }
        }
        public static readonly DependencyProperty UseLightingProperty =
            DependencyProperty.Register(
                "UseLighting",
                typeof(bool),
                typeof(LightingShaderProgram),
                new PropertyMetadata(true));
        #endregion UseLighting

        #region UseSpecular
        /// <summary> Whether to add specular highlights </summary>
        [ShaderProperty("UseSpecular")]
        public bool UseSpecular
        {
            get { return (bool)GetValue(UseSpecularProperty); }
            set { SetValue(UseSpecularProperty, value); }
        }
        public static readonly DependencyProperty UseSpecularProperty =
            DependencyProperty.Register(
                "UseSpecular",
                typeof(bool),
                typeof(LightingShaderProgram),
                new PropertyMetadata(true));
        #endregion UseSpecular

        #region Texture1
        /// <summary> The texture to apply to surfaces </summary>
        [ShaderProperty("texture1", OpenTK.Graphics.TextureUnit.Texture0)]
        public Texture Texture1
        {
            get { return (Texture)GetValue(Texture1Property); }
            set { SetValue(Texture1Property, value); }
        }
        public static readonly DependencyProperty Texture1Property =
            DependencyProperty.Register("Texture1", typeof(Texture), typeof(LightingShaderProgram), new UIPropertyMetadata(null));
        #endregion

        public LightingShaderProgram()
        {
            Initialize();
        }

        public override void Activate()
        {
            // Move the light
            LightingOffset += LightingOffsetDelta;
            if (LightingOffset > PI2)
                LightingOffset -= PI2;

            base.Activate();
        }

        public void Initialize()
        {
            var file = new FileInfo(@"Shaders\Simple_VS.glsl");
            this.VertexShader = new VertexShader(file);
            file = new FileInfo(@"Shaders\Simple_FS.glsl");
            this.FragmentShader = new FragmentShader(file);
        }
    }
}
