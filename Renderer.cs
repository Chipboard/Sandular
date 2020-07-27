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

        public static bool RawPlot = true;
        public static bool RoundPlot = false;

        public static void RenderTick()
        {
            Program.ProgramWindow.Clear(Color.Black);

            BackgroundImage.Dispose();
            BackgroundImage = new Image(Background.Texture.Size.X, Background.Texture.Size.Y);

            //Render particles
            for (int i = 0; i < Solver.Particles.Count; i++)
            {
                if (RoundPlot)
                {
                    if (RawPlot)
                        PlotPointRaw((uint)Math.Round(Solver.Particles[i].PX), (uint)Math.Round(Solver.Particles[i].PY), VariateColor(Resources.PowderTypes[(int)Solver.Particles[i].Type].Color, Solver.Particles[i].ColVar));
                    else
                        PlotPoint((uint)Math.Round(Solver.Particles[i].PX), (uint)Math.Round(Solver.Particles[i].PY), VariateColor(Resources.PowderTypes[(int)Solver.Particles[i].Type].Color, Solver.Particles[i].ColVar));
                } else
                {
                    if (RawPlot)
                        PlotPointRaw((uint)Solver.Particles[i].PX, (uint)Solver.Particles[i].PY, VariateColor(Resources.PowderTypes[(int)Solver.Particles[i].Type].Color, Solver.Particles[i].ColVar));
                    else
                        PlotPoint((uint)Solver.Particles[i].PX, (uint)Solver.Particles[i].PY, VariateColor(Resources.PowderTypes[(int)Solver.Particles[i].Type].Color, Solver.Particles[i].ColVar));
                }
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

                Program.FPSText.DisplayedString = Program.FPS.ToString();
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

        public static Color VariateColor(Color Col, byte Var)
        {
            byte Rem = (byte)(Var * 0.5f);
            return new Color((byte)(Col.R - Rem), (byte)(Col.G - Rem), (byte)(Col.B - Rem), Col.A);
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

        public static void PlotPointRaw(uint X, uint Y, Color Col)
        {
            BackgroundImage.SetPixel(X, Y, Col);
        }
    }
}
