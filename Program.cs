using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Collections.Generic;
using System.Threading;

namespace Sandular
{
    class Program
    {
        public static RenderWindow ProgramWindow;
        public static Vector2u Resolution;
        public static VideoMode VideoMode;
        public static Clock FPSClock;
        public static float FPS;

        public static bool ShowGrid = false;
        public static bool ShowFPS = true;
        public static Text FPSText;
        public static Font Font;

        public static bool Paused;

        static void Main(string[] args)
        {
            Initialize();

            while (ProgramWindow.IsOpen)
            {
                if (ProgramWindow.HasFocus())
                {
                    //Dispatch the events before checking them
                    ProgramWindow.DispatchEvents();

                    //Dispatch event to OnClose() function when window close is pressed
                    ProgramWindow.Closed += (sender, e) => { OnClose(); };

                    //Send scroll wheel to input handler
                    ProgramWindow.MouseWheelMoved += (sender, e) => { InputHandler.OnMouseScroll(sender, e); };

                    //Send keyboard input to input handler
                    ProgramWindow.KeyPressed += (sender, e) => { InputHandler.OnKeyPressed(sender, e); };
                    ProgramWindow.KeyReleased += (sender, e) => { InputHandler.OnKeyReleased(sender, e); };

                    //Get the input
                    InputHandler.Tick();

                    if (!Paused)
                    {
                        //Tick the solver
                        Solver.Tick();
                    }

                    //Render a frame
                    Renderer.RenderTick();

                    if (ShowFPS)
                    {
                        long Micro = FPSClock.Restart().AsMicroseconds();
                        if (Micro > 0)
                        {
                            FPS = 1000000 / Micro;
                        }
                        else
                        {
                            FPS = float.PositiveInfinity;
                        }
                    }
                }
            }
        }

        static void Initialize()
        {
            //Initialize the game
            Font = new Font("Content/Fonts/zrnic.ttf");
            FPSClock = new Clock();
            InitializeWindow();
            SetBackground();
            Resources.Initialize();

            Solver.Particles = new List<PowderPoint>((int)(Resolution.X * Resolution.Y));
            Solver.GridPoints = new Dictionary<Vector2u, GridPoint>();

            Renderer.GridDraw = new RectangleShape()
            {
                OutlineColor = new Color(0, 255, 15, 190),
                OutlineThickness = 1,
                FillColor = new Color(0, 120, 215, 100),
                Size = new Vector2f(Solver.GridSize - 1, Solver.GridSize - 1)
            };
            FloatRect Rect = Renderer.GridDraw.GetLocalBounds();
            Renderer.GridDraw.Origin = new Vector2f(Rect.Width / 2, Rect.Height / 2);

            Renderer.CursorColor = new Color(255, 255, 255, 150);

            //Solver.GravityPoint = (Vector2f)(Resolution / 2);
        }

        static void InitializeWindow()
        {
            Resolution = new Vector2u(250, 250);
            VideoMode = new VideoMode(Resolution.X, Resolution.Y, 24);
            ProgramWindow = new RenderWindow(VideoMode, "Sandular", Styles.Close);
            ProgramWindow.Size = Resolution * 2;
            //ProgramWindow.SetVerticalSyncEnabled(true);
            ProgramWindow.SetFramerateLimit(120);
            ProgramWindow.SetKeyRepeatEnabled(false);
        }

        static void SetBackground()
        {
            Renderer.BackgroundImage = new Image(Resolution.X, Resolution.Y, Color.Blue);
            Renderer.Background = CreateSprite(Renderer.BackgroundImage);
            Renderer.Background.Scale = new Vector2f((float)Resolution.X / Renderer.Background.Texture.Size.X, (float)Resolution.Y / Renderer.Background.Texture.Size.Y);

            Renderer.Background.Position = (Vector2f)Resolution / 2;
        }

        static void OnClose()
        {
            //Called when window is closed
            ProgramWindow.Close();
        }

        public static Sprite CreateSprite(Image Img, bool Centered = true)
        {
            Texture Tex = new Texture(Resolution.X, Resolution.Y);
            Sprite NewSprite = new Sprite(Tex);

            if (Centered)
            {
                NewSprite.Origin = new Vector2f(Tex.Size.X / 2, Tex.Size.Y / 2);
            }

            return NewSprite;
        }
    }
}
