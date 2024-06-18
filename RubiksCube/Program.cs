using System;
using System.Threading;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

namespace RubiksCube
{
    class Game : GameWindow
    {
        private readonly Cube cube;
        private Vector3d angle;
        private readonly Queue<Action> rotations = new Queue<Action>();
        private readonly Thread thread;
        private readonly Dictionary<Key, Action> keys = new Dictionary<Key, Action>();
    public Game() : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
            thread = new Thread(new ThreadStart(() => { while (true) if (rotations.Count > 0) rotations.Dequeue()(); }));
            thread.Start();
            cube = new Cube(50);
            keys[Key.W] = rotationX(1);
            keys[Key.S] = rotationX(-1);
            keys[Key.A] = rotationY(-1);
            keys[Key.D] = rotationY(1);
            keys[Key.Up] = rotationSelected(3);
            keys[Key.Down] = rotationSelected(2);
            keys[Key.Left] = rotationSelected(0);
            keys[Key.Right] = rotationSelected(1);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUnload(EventArgs e) => 
            Environment.Exit(0);

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left || e.Button == MouseButton.Right)
                rotations.Enqueue(rotationY(e.Button == MouseButton.Left ? -1 : 1));
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta != 0)
                rotations.Enqueue(rotationX(Math.Sign(e.Delta)));
        }

        private Action rotationX(int sign) =>
            () => { for (var i = 0; i < 30; i++) { angle.X += sign * 3; Thread.Sleep(10); } Rotate(); };

        private Action rotationY(int sign) =>
            () => { for (var i = 0; i < 30; i++) { angle.Y += sign * 3; Thread.Sleep(10); } Rotate(); };

        private Action rotationSelected(int n) =>
            () => cube.RotateSelected(n);

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.LookAt(new Vector3(0, 0, -5), Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Rotate(angle.X, 1, 0, 0);
            GL.Rotate(angle.Y, 0, 1, 0);
            cube.Draw(Draw);
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Tab)
                cube.ChangeSelection(e.Shift ? -1 : 1);
            else if (keys.ContainsKey(e.Key))
                rotations.Enqueue(keys[e.Key]);
        }

        private void Draw(Color color, IEnumerable<Vector3> vertexes, BeginMode mode)
        {
            GL.Begin(mode);
            GL.Color3(color);
            foreach(var v in vertexes)
                GL.Vertex3(v);
            GL.End();
        }

        private void Rotate()
        {
            //if (Math.Abs(angle.X) > 0)
                cube.Rotate(Math.PI / 2 * Math.Sign(angle.X), Math.PI / 2 * Math.Sign(angle.Y), 0);
            //else
                //cube.Rotate(0, , 0);
            angle.X = 0;
            angle.Y = 0;
        }

        static void Main()
        {
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}