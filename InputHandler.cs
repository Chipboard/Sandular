using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace Sandular
{
    class InputHandler
    {
        public static Vector2f MousePosition;
        public static int Type = 0;
        public static float RangeX = 5;
        public static float RangeY = 5;

        public static Dictionary<Keyboard.Key, bool> KeyValues = new Dictionary<Keyboard.Key, bool>();

        public static void Tick()
        {
            MousePosition = Program.ProgramWindow.MapPixelToCoords(Mouse.GetPosition(Program.ProgramWindow));

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                int MinX = (int)InputHandler.MousePosition.X - (int)InputHandler.RangeX + 1;
                int MinY = (int)InputHandler.MousePosition.Y - (int)InputHandler.RangeY + 1;
                int MaxX = (int)InputHandler.MousePosition.X + (int)InputHandler.RangeX;
                int MaxY = (int)InputHandler.MousePosition.Y + (int)InputHandler.RangeY;

                for (int CX = MinX; CX < MaxX; CX++)
                {
                    for (int CY = MinY; CY < MaxY; CY++)
                    {
                        Solver.Insert(new Vector2i(CX, CY), Type);
                    }
                }
            }

            if (Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                int MinX = (int)MousePosition.X - (int)RangeX + 1;
                int MinY = (int)MousePosition.Y - (int)RangeY + 1;
                int MaxX = (int)MousePosition.X + (int)RangeX;
                int MaxY = (int)MousePosition.Y + (int)RangeY;

                for (int CX = MinX; CX < MaxX; CX++)
                {
                    for (int CY = MinY; CY < MaxY; CY++)
                    {
                        Solver.Remove(new Vector2i(CX, CY), Type);
                    }
                }
            }
        }

        public static void KeyPressed(Keyboard.Key Key)
        {
            if (Key == Keyboard.Key.F)
                Program.ShowFPS = !Program.ShowFPS;

            if (Key == Keyboard.Key.E)
                Type++;

            if (Key == Keyboard.Key.Q)
                Type--;

            /*if (Key == Keyboard.Key.T)
                Renderer.RoundPlot = !Renderer.RoundPlot;

            if (Key == Keyboard.Key.R)
                Renderer.RawPlot = !Renderer.RawPlot;*/

            if (Key == Keyboard.Key.G)
                Program.ShowGrid = !Program.ShowGrid;

            if(Key == Keyboard.Key.Space)
                Program.Paused = !Program.Paused;

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && Program.ShowGrid)
            {
                if (Key == Keyboard.Key.Up)
                {
                    if (Solver.GridSize <= 40)
                        Solver.GridSize += 5;

                    Solver.HalfGrid = Solver.GridSize / 2;

                    FloatRect Rect = Renderer.GridDraw.GetLocalBounds();
                    Renderer.GridDraw.Origin = new Vector2f(Rect.Width / 2, Rect.Height / 2);
                }

                if (Key == Keyboard.Key.Down)
                {
                    if (Solver.GridSize >= 10)
                        Solver.GridSize -= 5;

                    Solver.HalfGrid = Solver.GridSize / 2;

                    FloatRect Rect = Renderer.GridDraw.GetLocalBounds();
                    Renderer.GridDraw.Origin = new Vector2f(Rect.Width / 2, Rect.Height / 2);
                }
            }

            if (Type > Resources.PowderTypes.Count - 1)
            {
                Type = 0;
            }
            else if (Type < 0)
            {
                Type = Resources.PowderTypes.Count - 1;
            }
        }

        public static void OnKeyPressed(object sender, EventArgs e)
        {
            KeyEventArgs KeyArgs = (KeyEventArgs)e;

            bool KeyValue;
            if(!KeyValues.TryGetValue(KeyArgs.Code, out KeyValue))
            {
                KeyValues.Add(KeyArgs.Code, true);
                KeyPressed(KeyArgs.Code);
            }
        }

        public static void OnKeyReleased(object sender, EventArgs e)
        {
            KeyEventArgs KeyArgs = (KeyEventArgs)e;

            KeyValues.Remove(KeyArgs.Code);
        }

        public static void OnMouseScroll(object sender, EventArgs e)
        {
            MouseWheelEventArgs MouseEvent = (MouseWheelEventArgs)e;

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                if (MouseEvent.Delta > 0)
                    RangeX += 0.001f;
                else
                    RangeX -= 0.001f;

                if (RangeX < 1)
                    RangeX = 1;

                if (RangeX > 50)
                    RangeX = 50;

                return;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                if(MouseEvent.Delta>0)
                    RangeY += 0.001f;
                else
                    RangeY -= 0.001f;

                if (RangeY < 1)
                    RangeY = 1;

                if (RangeY > 50)
                    RangeY = 50;

                return;
            }

            Type += MouseEvent.Delta;
            if (Type > Resources.PowderTypes.Count - 1)
            {
                Type = 0;
            }
            else if (Type < 0)
            {
                Type = Resources.PowderTypes.Count - 1;
            }
        }
    }
}
