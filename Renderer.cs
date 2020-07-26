using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Sandular
{
    class Renderer
    {
        public static Sprite Background;
        public static Image BackgroundImage;
        public static RectangleShape GridDraw;
        public static Color CursorColor;

        public static void RenderTick()
        {
            Program.ProgramWindow.Clear(Color.Black);

            BackgroundImage.Dispose();
            BackgroundImage = new Image(Background.Texture.Size.X, Background.Texture.Size.Y);

            //Render particles
            for (int i = 0; i < Solver.Particles.Count; i++)
            {
                PlotPoint((uint)Math.Round(Solver.Particles[i].PX), (uint)Math.Round(Solver.Particles[i].PY), Resources.PowderTypes[(int)Solver.Particles[i].Type].Color);
            }

            //Render cursor
            int MinX = (int)InputHandler.MousePosition.X - (int)InputHandler.RangeX+1;
            int MinY = (int)InputHandler.MousePosition.Y - (int)InputHandler.RangeY+1;
            int MaxX = (int)InputHandler.MousePosition.X + (int)InputHandler.RangeX;
            int MaxY = (int)InputHandler.MousePosition.Y + (int)InputHandler.RangeY;

            for (int CX = MinX; CX < MaxX; CX++)
            {
                for (int CY = MinY; CY < MaxY; CY++)
                {
                    if(CX == MinX || CX == MaxX-1 || CY == MinY || CY == MaxY-1)
                    PlotPoint((uint)CX, (uint)CY, CursorColor);
                }
            }

            Background.Texture.Update(BackgroundImage);

            Program.ProgramWindow.Draw(Background);

            if (Program.ShowFPS)
            {
                if (Program.FPSText == null)
                {
                    Program.FPSText = new Text(Program.FPS.ToString(), Program.Font, 10);
                }

                Program.FPSText.DisplayedString = Solver.Particles.Count.ToString();
                Program.ProgramWindow.Draw(Program.FPSText);
            }

            if (Program.ShowGrid)
            {
                foreach (KeyValuePair<Vector2u, GridPoint> Entry in Solver.GridPoints)
                {
                    GridDraw.Size = new Vector2f(Solver.GridSize - 1, Solver.GridSize - 1);
                    GridDraw.Position = (Vector2f)Entry.Key;
                    Program.ProgramWindow.Draw(GridDraw);
                }
            }

            Program.ProgramWindow.Display();
        }

        public static void PlotPoint(uint X, uint Y, Color Col)
        {
            if (X > 0 && X <= BackgroundImage.Size.X-1)
            {
                if (Y > 0 && Y <= BackgroundImage.Size.Y-1)
                {
                    BackgroundImage.SetPixel(X, Y, Col);
                }
            }
        }
    }
}
