using SFML.Graphics;
using System.Collections.Generic;

namespace Sandular
{
    class Resources
    {
        public static List<PowderType> PowderTypes = new List<PowderType>();
        public static void Initialize()
        {
            PowderTypes.Add(new PowderType()
            {
                Name = "Sand",
                Color = new Color(226,226,44,255),
                Drag = 0.05f,
                Mass = 0.055f,
                MaxColVar = 85
            });

            /*PowderTypes.Add(new PowderType()
            {
                Name = "Water",
                Color = new Color(59, 140, 220, 200),
                Drag = 0.0001f,
                Liquid = true,
                Mass = 0.036126842f,
                MaxColVar = 75
            });*/

            PowderTypes.Add(new PowderType()
            {
                Name = "Wall",
                Color = new Color(101, 102, 103, 255),
                Drag = -1,
                MaxColVar = 45
            });
        }
    }

    class PowderPoint
    {
        public float PX;
        public float PY;

        public float VX;
        public float VY;

        public uint Type;

        public byte ColVar;
    }

    class PowderType
    {
        public string Name;
        public Color Color;
        public float Drag;
        public bool Liquid;
        public float Mass;
        public byte MaxColVar;
    }
}
