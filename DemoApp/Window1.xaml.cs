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

using System.Windows.Threading;
using Draw = System.Drawing; // used for generating the sample textures
using OpenGL;
using OpenGL.Geometry;
using OpenTK.Math;
using OpenTK.Graphics;
using System.IO;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Serialization;
using WPFOpenTK;

namespace DemoApp
{
    /// <summary> A demo Window with a GLElement. See <c>RenderToBuffer()</c> for the beef. </summary>
    public partial class Window1 : Window
    {
        #region Fields
        DisplayList boxList, cylinderList, sphereList;
        DispatcherTimer dTimer = new DispatcherTimer(DispatcherPriority.Render);
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        #endregion

        #region Properties
        #region WrippleShaderProgram
        public WrippleShaderProgram WrippleShaderProgram
        {
            get { return (WrippleShaderProgram)GetValue(WrippleShaderProgramProperty); }
            set { SetValue(WrippleShaderProgramProperty, value); }
        }
        public static readonly DependencyProperty WrippleShaderProgramProperty =
            DependencyProperty.Register("WrippleShaderProgram", typeof(WrippleShaderProgram), typeof(Window1), new UIPropertyMetadata(null));
        #endregion

        #region LightingShaderProgram
        public LightingShaderProgram LightingShaderProgram
        {
            get { return (LightingShaderProgram)GetValue(LightingShaderProgramProperty); }
            set { SetValue(LightingShaderProgramProperty, value); }
        }
        public static readonly DependencyProperty LightingShaderProgramProperty =
            DependencyProperty.Register("LightingShaderProgram", typeof(LightingShaderProgram), typeof(Window1), new UIPropertyMetadata(null));
        #endregion

        #region PatternShaderProgram
        public PatternShaderProgram PatternShaderProgram
        {
            get { return (PatternShaderProgram)GetValue(PatternShaderProgramProperty); }
            set { SetValue(PatternShaderProgramProperty, value); }
        }
        public static readonly DependencyProperty PatternShaderProgramProperty =
            DependencyProperty.Register("PatternShaderProgram", typeof(PatternShaderProgram), typeof(Window1), new UIPropertyMetadata(null));
        #endregion
        #endregion

        #region Texture and Framebuffer Object Stuff
        Texture2D CirclesTexture;
        Texture2D FileTexture;
        Draw.Bitmap MakeImageCircles(int imgSize)
        {
            var bmp = new Draw.Bitmap(imgSize, imgSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (var g = Draw.Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Draw.Color.FromArgb(20, Draw.Color.Black));
                using (var brush = new Draw.SolidBrush(Draw.Color.FromArgb(50, Draw.Color.Blue)))
                {
                    g.FillEllipse(brush, imgSize / 6, imgSize / 6, imgSize * 2 / 3, imgSize * 2 / 3);
                    g.FillEllipse(brush, imgSize / 6, imgSize, imgSize / 4, imgSize / 4);
                }
                g.FillEllipse(Draw.Brushes.Chartreuse, imgSize / 4, imgSize / 4, imgSize / 2, imgSize / 2);
                g.FillEllipse(Draw.Brushes.Red, imgSize / 8, imgSize / 8, imgSize / 4, imgSize / 4);
                g.FillEllipse(Draw.Brushes.Blue, imgSize / 6, 7 * imgSize / 8, imgSize / 4, imgSize / 4);
            }
            return bmp;
        }

        FrameBuffer FBO;
        public TransformGL TransformOuter { get { return (TransformGL)Resources["TransformOuter"]; } }
        #region UseFrameBuffer
        public bool UseFrameBuffer
        {
            get { return (bool)GetValue(UseFrameBufferProperty); }
            set { SetValue(UseFrameBufferProperty, value); }
        }
        // Using a DependencyProperty as the backing store for UseFrameBuffer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UseFrameBufferProperty =
            DependencyProperty.Register("UseFrameBuffer", typeof(bool), typeof(Window1), new UIPropertyMetadata(false));
        #endregion
        #endregion

        public Window1()
        {
            InitializeComponent();
            GLElement1.GLMouseMove += new System.Windows.Forms.MouseEventHandler(Child_MouseMove);
            GLElement1.GLMouseUp += new System.Windows.Forms.MouseEventHandler(Child_MouseUp);
            GLElement1.GLMouseDown += new System.Windows.Forms.MouseEventHandler(Child_MouseDown);
            GLElement1.GLMouseLeave += new EventHandler(Child_MouseLeave);
            GLElement1.GLMouseWheel += new System.Windows.Forms.MouseEventHandler(Child_MouseWheel);

            // not sure if I need this anymore
            //dTimer.Interval = TimeSpan.FromMilliseconds(1);
            //dTimer.Tick += new EventHandler(dTimer_Tick);
        }

        #region OpenGL Events
        private void GLElement_GLInitialized(object sender, EventArgs e)
        {
            InitializeShaders();

            // Initialize textures
            CirclesTexture = new Texture2D(MakeImageCircles(512), false);
            var bitmap = Draw.Bitmap.FromFile(@"Toco Toucan.jpg", true) as System.Drawing.Bitmap;
            FileTexture = new Texture2D(bitmap, false);
            Texture2D.Disable2DTextures();

            // Initialize SFM
            boxList = new DisplayList();
            boxList.Initialize(() => new Box(100000).Draw());
            cylinderList = new DisplayList();
            cylinderList.Initialize(() => new Cylinder(100000).Draw());
            sphereList = new DisplayList();
            sphereList.Initialize(() => new Sphere(100000).Draw());

            //InitializeView();
            int size = (int)Math.Min(GLElement1.ActualWidth, GLElement1.ActualHeight);
            FBO = new FrameBuffer();
            FBO.Initialize(size, size);
        }

        private void GLElement_GLRenderStarted(object sender, EventArgs e)
        {
            #region FBO start
            if (UseFrameBuffer)
            {
                FBO.Activate();
                FBO.FBOInitializeView(GLElement1.PerspectiveProjection, TransformOuter.Location.Z);
            }
            else
            {
                FBO.Deactivate();
            }
            #endregion

            RenderToBuffer();

            #region FBO finish
            if (UseFrameBuffer)
            {
                FBO.Deactivate();

                GLElement1.InitializeView();
                GL.ClearColor(0, .3f, .5f, 1);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.LoadIdentity();

                // Now bind the texture to use it
                GL.ActiveTexture(TextureUnit.Texture0);
                FBO.Bind();
                GLHelpers.Color4(Colors.Black); // to prevent it from being transparent

                // Tranform
                TransformOuter.Apply();

                // Render with shader
                LightingShaderProgram.Texture1 = FBO.Texture;
                LightingShaderProgram.Activate();
                //GLHelpers.Plane(1.3, 1.3, 0); // put it on a plane
                GLHelpers.Cube2D(1.7); // put it on a cube

                // Do some cleanup
                TransformOuter.Remove();
                Texture2D.Disable2DTextures();
                ShaderProgram.DisableShaders();
            }
            #endregion
        }

        private void GLElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // somehow, this event fires before the GLElement completely loads
            if (FBO == null)
                return;

            FBO.Width = FBO.Height = (int) (Math.Min(GLElement1.ActualWidth, GLElement1.ActualHeight) * 1);
        } 
        #endregion

        #region OpenGL Methods
        /// <summary>Render the inner scene</summary>
        private void RenderToBuffer()
        {
            // transform
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GLElement1.TransformInner.Apply();

            // set up
            GLHelpers.ClearColor(GLElement1.ClearColor);
            GLHelpers.Color4(Colors.White);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.PointSmooth);

            // render the content
            if (chkSFM.IsChecked.HasValue && chkSFM.IsChecked.Value)
                RenderSFM();
            if (chkPatternCube.IsChecked.HasValue && chkPatternCube.IsChecked.Value)
                RenderPatternCube();
            if (chkTexturedCube.IsChecked.HasValue && chkTexturedCube.IsChecked.Value)
                RenderTexturedCube(chkLightCubes.IsChecked.HasValue & chkLightCubes.IsChecked.Value);
            if (chkTexturedPlane.IsChecked.HasValue && chkTexturedPlane.IsChecked.Value)
                RenderTexturedPlane();
            if (chkMouseControl.IsChecked.HasValue && chkMouseControl.IsChecked.Value)
                DrawUserStudySelection();

            // cleanup
            GLElement1.TransformInner.Remove();
            ShaderProgram.DisableShaders();
            GL.ActiveTexture(TextureUnit.Texture0);
            Texture2D.Disable2DTextures();
        }

        #region Render Content
        private void RenderSFM()
        {
            Texture2D.Disable2DTextures();
            WrippleShaderProgram.Activate();

            GL.PushMatrix();
            GL.PushAttrib(AttribMask.PointBit);
            //GL.PointSize((float)GLElement1.ActualWidth / 500);
            GL.PointSize(2);

            WrippleShaderProgram.ColorX = Colors.Red;
            WrippleShaderProgram.ColorY = Colors.Lime; // Why did MS call it lime?
            WrippleShaderProgram.ColorZ = Colors.Blue;
            WrippleShaderProgram.UpdateProperties();
            sphereList.Draw();

            //WrippleShaderProgram.ColorX = Colors.Magenta;
            //WrippleShaderProgram.ColorY = Colors.Yellow;
            //WrippleShaderProgram.ColorZ = Colors.Cyan;
            //WrippleShaderProgram.UpdateProperties();
            //cylinderList.Draw();
            //boxList.Draw();

            GL.PopAttrib();
            GL.PopMatrix();
            ShaderProgram.DisableShaders();
        } 
        private void RenderPatternCube()
        {
            PatternShaderProgram.Activate();
            GL.PushMatrix();
            GL.Scale(.35, .35, .35);
            GLHelpers.Cube2D(1.99f);
            GL.PopMatrix();

            ShaderProgram.DisableShaders();
        }
        private void RenderTexturedPlane()
        {
            GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            LightingShaderProgram.Texture1 = FileTexture;
            //FileTexture.Bind();
            LightingShaderProgram.Activate();

            GL.PushMatrix();
            float width = FileTexture.Picture.Width / (float)FileTexture.Picture.Height;
            GLHelpers.PlaneMesh(width, 1, 3, -0.25f);
            GL.PopMatrix();

            ShaderProgram.DisableShaders();
            GL.PopAttrib();
        }
        private void RenderTexturedCube(bool useShader)
        {
            GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            if (useShader)
            {
                GLHelpers.Color4(Colors.Transparent);
                LightingShaderProgram.Texture1 = CirclesTexture;
                LightingShaderProgram.Activate();
            }
            else
            {
                GLHelpers.Color4(Colors.Black);
                GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Replace);
                //GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Decal);
                CirclesTexture.Bind();
            }

            var transformCube = new TransformGL(new Vector3(-.4f, -.4f, 0));

            transformCube.Apply();
            GLHelpers.Cube2D(.5);
            transformCube.Remove();

            transformCube.Location = new Vector3(0, 0, 0);
            transformCube.Apply();
            GLHelpers.Cube2D(.25);
            transformCube.Remove();

            transformCube.Location = new Vector3(.2f, .2f, 0);
            transformCube.Apply();
            GLHelpers.Cube2D(.125);
            transformCube.Remove();

            transformCube.Location = new Vector3(.3f, .3f, 0);
            transformCube.Apply();
            GLHelpers.Cube2D(.06125);
            transformCube.Remove();

            // cleanup
            ShaderProgram.DisableShaders();
            GL.PopAttrib();
        }
        #endregion

        /// <summary>Load the shaders from disk and set them up</summary>
        private void InitializeShaders()
        {
            WrippleShaderProgram = new WrippleShaderProgram();
            LightingShaderProgram = new LightingShaderProgram();
            PatternShaderProgram = new PatternShaderProgram();
        }
        #endregion

        #region Events
        private void btnScreenshot_Click(object sender, RoutedEventArgs e)
        {
            saveFileDialog.Filter = "PNG File|*.png|Jpeg File|*.jpg|GIF File|*.gif|Bitmap File|*.bmp|All images|*.png,*.bmp,*.jpg;\"";
            if (!saveFileDialog.ShowDialog().Value)
                return;

            var bitmap = GLElement1.Screenshot();
            bitmap.Save(saveFileDialog.FileName);

            var story = Resources["storyScreenshotTaken"] as Storyboard;
            story.Begin();
        } 

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            //dTimer.Start();
        }

        void dTimer_Tick(object sender, EventArgs e)
        {
            // not sure if I need the updates anymore
            this.RotationEdit.Update();
            this.LocationEdit.Update();
            this.ScaleEdit.Update();
            GLElement1.TransformInner.Update();
        }
        #endregion

        #region User Study
        #region User Study Mouse
        Box2 MouseQuadrant = new Box2();
        Box2 SelectedQuadrant = new Box2();

        void Window1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
            e.Handled = true;
        }

        void Child_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var control = sender as System.Windows.Forms.Control;
            var position = e.Location;
            if (position.X > control.Width / 2)
            {
                MouseQuadrant.Left = 0;
                MouseQuadrant.Right = 1f;
            }
            else
            {
                MouseQuadrant.Left = -1f;
                MouseQuadrant.Right = 0;
            }
            if (position.Y > control.Height / 2)
            {
                MouseQuadrant.Top = 0;
                MouseQuadrant.Bottom = -1f;
            }
            else
            {
                MouseQuadrant.Top = 1f;
                MouseQuadrant.Bottom = 0;
            }

            var sizeDifference = Math.Abs(control.Width - control.Height) / 2;
            if (control.Height > control.Width &&
                (position.Y < sizeDifference || position.Y > control.Width + sizeDifference))
            {
                ResetMouseQuadrant();
            }
            if (control.Width > control.Height &&
                (position.X < sizeDifference || position.X > control.Height + sizeDifference))
            {
                ResetMouseQuadrant();
            }
        }
        void Child_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            SelectedQuadrant = MouseQuadrant;
        }
        void Child_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        void Child_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }
        private void window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
        }
        void Child_MouseLeave(object sender, EventArgs e)
        {
            ResetMouseQuadrant();
        }
        private void ResetMouseQuadrant()
        {
            MouseQuadrant.Left = MouseQuadrant.Right = MouseQuadrant.Top = MouseQuadrant.Bottom = 0;
        }
        private void DrawUserStudySelection()
        {
            GL.LineWidth(4);
            GLHelpers.Color4(Color.FromArgb(50, 255, 0, 0));
            Texture2D.Disable2DTextures();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            using (OpenGL.GLDraw.Begin(BeginMode.Quads))
            {
                GL.Vertex3(MouseQuadrant.Left, MouseQuadrant.Top, -.02f);
                GL.Vertex3(MouseQuadrant.Right, MouseQuadrant.Top, -.02f);
                GL.Vertex3(MouseQuadrant.Right, MouseQuadrant.Bottom, -.02f);
                GL.Vertex3(MouseQuadrant.Left, MouseQuadrant.Bottom, -.02f);
            }
            GLHelpers.Color4(Color.FromArgb(50, 255, 0, 0));
            GL.LineWidth(12);
            using (OpenGL.GLDraw.Begin(BeginMode.Quads))
            {
                GL.Vertex3(SelectedQuadrant.Left, SelectedQuadrant.Top, -.01f);
                GL.Vertex3(SelectedQuadrant.Right, SelectedQuadrant.Top, -.01f);
                GL.Vertex3(SelectedQuadrant.Right, SelectedQuadrant.Bottom, -.01f);
                GL.Vertex3(SelectedQuadrant.Left, SelectedQuadrant.Bottom, -.01f);
            }
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }
        #endregion 
        #endregion
    }
}
