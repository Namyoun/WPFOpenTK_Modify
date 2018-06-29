using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace OpenGL.Geometry
{
    /// <summary> A base class for renderable objects </summary>
    /// <remarks> Find a way to make GraphicsResource an interface and inherit from it </remarks>
    public abstract class Geometry : DependencyObject
    {
        public virtual void Initialize() { }
        public abstract void Draw();
    }
}
