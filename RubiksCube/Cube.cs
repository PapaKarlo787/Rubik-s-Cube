using System;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace RubiksCube
{
    class Cube
    {
        private readonly List<CubePart> parts = new List<CubePart>();
        private readonly List<Arrow> arrows = new List<Arrow>();
        private readonly float size;
        private readonly int n;
        private int selected;
        public Cube(int n)
        {
            this.n = n;
            size = 1.0f / n;
            for (var i = 0; i < n; i++)
                for (var l = 0; l < n; l++)
                    for (var k = 0; k < n; k++)
                        if (i % (n - 1) == 0 || l % (n - 1) == 0 || k % (n - 1) == 0)
                            parts.Add(new CubePart(new Vector3(Put(n, i), Put(n, l), Put(n, k)), size));
            var z = Put(n, 0);
            for (var i = 0; i < n; i++)
            {
                var current = new Vector3(Put(n, 0) - 2 * size, Put(n, i), z);
                var a = new Vector3(-size * 2, 0, 0);
                var b = new Vector3(0, -size * 0.7f, 0);
                var c = new Vector3(0, size * 0.7f, 0);
                arrows.Add(new Arrow(new[] { a + current, b + current, c + current })); // Left
                current = new Vector3(Put(n, n - 1) + 2 * size, Put(n, i), z);
                arrows.Add(new Arrow(new[] { current - a, b + current, c + current })); // Right
                current = new Vector3(Put(n, i), Put(n, 0) - 2 * size, z);
                a = new Vector3(0, -size * 2, 0);
                b = new Vector3(-size * 0.7f, 0, 0);
                c = new Vector3(size * 0.7f, 0, 0);
                arrows.Add(new Arrow(new[] { a + current, b + current, c + current })); // Down
                current = new Vector3(Put(n, i), Put(n, n - 1) + 2 * size, z);
                arrows.Add(new Arrow(new[] { current - a, b + current, c + current })); // Up
            }
            ChangeSelection(-1);
        }

        private float Put(int total, int current) => size * 1.02f * (1 - total + current * 2);

        public void Rotate(double ux, double uy, double uz) =>
            parts.ForEach(p => p.Rotate(ux, uy, uz));

        public void RotateSelected(int n)
        {
            var angle = -Math.PI / 60 * Math.Pow(-1, n);
            Func<CubePart, bool> x = p => Math.Abs(p.center.X - arrows[selected * 4 + n].GetTail().X) < 0.001;
            Func<CubePart, bool> y = p => Math.Abs(p.center.Y - arrows[selected * 4 + n].GetTail().Y) < 0.001;
            for (var i = 0; i < 30; i++)
            {
                foreach (var p in parts.Where(n > 1 ? x : y))
                    if (n > 1) p.Rotate(angle, 0, 0);
                    else p.Rotate(0, angle, 0);
                Thread.Sleep(10);
            }

        }

        public void Draw(Action<Color, IEnumerable<Vector3>, BeginMode> drawer)
        {
            parts.ForEach(p => p.Draw(drawer));
            arrows.ForEach(a => a.Draw(drawer));
        }

        public void ChangeSelection(int sign)
        {
            for (var i = 0; i < 4; i++)
                arrows[selected * 4 + i].Actived = false;
            selected = (selected + n - sign) % n;
            for (var i = 0; i < 4; i++)
                arrows[selected * 4 + i].Actived = true;
        }
    }
}
