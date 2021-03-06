﻿using System.Runtime.InteropServices;

namespace NetTopologySuite.IO.ShapeWrappers
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PointXY
    {
        public double X { get; set; }

        public double Y { get; set; }
    }
}
