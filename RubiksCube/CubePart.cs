using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Linq;
using OpenTK;

namespace RubiksCube
{
    class CubePart : Part
    {
        public CubePart(Vector3 center, double size) : base(center, BeginMode.Quads)
        {
            vertexes = new[] { new Vector3(-1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, -1, 1), new Vector3(-1, -1, 1),
                new Vector3(-1, 1, -1), new Vector3(1, 1, -1), new Vector3(1, -1, -1), new Vector3(-1, -1, -1),};
            edges = new[] { new[] { 0, 1, 2, 3 }, new[] { 1, 2, 6, 5 }, new[] { 4, 5, 6, 7 },
                new[] { 4, 7, 3, 0 },  new[] { 0, 1, 5, 4 }, new[] { 3, 2, 6, 7 } };
            colors = new[] { Color.Red, Color.Blue, Color.Orange, Color.Green, Color.White, Color.Yellow };
            vertexes = vertexes.Select(v => v * (float)size).ToArray();
        }
    }
}
