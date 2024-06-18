using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace RubiksCube
{
    class Arrow
    {
        private readonly IEnumerable<Vector3> vertexes;
        public bool Actived { get; set; }
        
        public Arrow(IEnumerable<Vector3> vertexes) =>
            this.vertexes = vertexes;

        public void Draw(Action<Color, IEnumerable<Vector3>, BeginMode> drawer) =>
            drawer(Actived ? Color.Red : Color.Orange, vertexes, BeginMode.Triangles);

        public Vector3 GetTail() => vertexes.First();
    }
}
