using SFML.System;
using SFML.Window;
using System;

namespace Sandular
{
    class InputHandler
    {
        public static Vector2f MousePosition;
        public static int Type = 0;
        public static float RangeX=5;
        public static float RangeY=5;

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
                int MinX = (int)InputHandler.MousePosition.X - (int)InputHandler.RangeX + 1;
                int MinY = (int)InputHandler.MousePosition.Y - (int)InputHandler.RangeY + 1;
                int MaxX = (int)InputHandler.MousePosition.X + (int)InputHandler.RangeX;
                int MaxY = (int)InputHandler.MousePosition.Y + (int)InputHandler.RangeY;

                for (int CX = MinX; CX < MaxX; CX++)
                {
                    for (int CY = MinY; CY < MaxY; CY++)
                    {
                        Solver.Remove(new Vector2i(CX, CY), Type);
                    }
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) && Program.ShowGrid)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    if(Solver.GridSize<=40)
                    Solver.GridSize+=5;
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
                {
                    if (Solver.GridSize >= 10)
                        Solver.GridSize-=5;
                }
            }
        }

        public static void OnMouseScroll(object sender, EventArgs e)
        {
            MouseWheelEventArgs MouseEvent = (MouseWheelEventArgs)e;

            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                RangeX += MouseEvent.Delta * 0.001f;

                if (RangeX < 1)
                    RangeX = 1;

                if (RangeX > 50)
                    RangeX = 50;

                return;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                RangeY += MouseEvent.Delta * 0.001f;

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
