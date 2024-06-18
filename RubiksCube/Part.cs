using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Linq;

namespace RubiksCube
{
    abstract class Part
    {
        protected Color[] colors;
        protected int[][] edges;
        protected Vector3[] vertexes;
        public Vector3 center;
        protected BeginMode mode;

        public Part(Vector3 center, BeginMode mode)
        {
            this.center = center;
            this.mode = mode;
        }

        public void Rotate(double ux, double uy, double uz)
        {
            center = Rotate(center, ux, uy, uz);
            vertexes = vertexes.Select(v => Rotate(v, ux, uy, uz)).ToArray();
        }

        public void Draw(Action<Color, IEnumerable<Vector3>, BeginMode> drawer)
        {
            for (var i = 0; i < edges.Length; i++)
            {
                var vertexes = new List<Vector3>();
                foreach (var e in edges[i])
                    vertexes.Add(this.vertexes[e]+center);
                drawer(colors[i], vertexes, mode);
            }
        }

        public static Vector3 Rotate(Vector3 me, double ux, double uy, double uz)
        {
            var x = me.X;
            var y = me.Y;
            var z = me.Z;
            Rotate(ux, ref y, ref z);
            Rotate(uy, ref z, ref x);
            Rotate(uz, ref x, ref y);
            return new Vector3(x, y, z);
        }

        private static void Rotate(double ungle, ref float coord1, ref float coord2)
        {
            var temp = (float)(coord1 * Math.Cos(ungle) - coord2 * Math.Sin(ungle));
            coord2 = (float)(coord1 * Math.Sin(ungle) + coord2 * Math.Cos(ungle));
            coord1 = temp;
        }
    }
}
