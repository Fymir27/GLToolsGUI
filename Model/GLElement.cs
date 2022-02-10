using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLElement
    {
        //Read from file
        public int Ref;
        public int Ndx;
        public uint Layer;
        public float a;
        public float b;
        public float g;
        public float r;
        public float uk1;
        public float uk2;
        public float uk3;
        public float uk4;
        public float m1;
        public float m2;
        public float m3;
        public float m4;
        public float mX;
        public float mY;
        public float index;

        //Calculated values
        public float X;
        public float Y;
        public float ScaleX;
        public float ScaleY;
        public double Angle;
        public float Spin;

        private float det;
        private bool IsFirst = true;

        public GLElement(GLReader reader, double prevAngle, float prevScaleX, float prevScaleY)
        {
            Ref = reader.ReadInt32();
            Ndx = reader.ReadInt32();
            Layer = reader.ReadUInt32();
            a = reader.ReadFloat();
            b = reader.ReadFloat();
            g = reader.ReadFloat();
            r = reader.ReadFloat();
            uk1 = reader.ReadFloat();
            uk2 = reader.ReadFloat();
            uk3 = reader.ReadFloat();
            uk4 = reader.ReadFloat();
            m1 = reader.ReadFloat();
            m2 = reader.ReadFloat();
            m3 = reader.ReadFloat();
            m4 = reader.ReadFloat();
            mX = reader.ReadFloat();
            mY = reader.ReadFloat();
            index = reader.ReadFloat();

            X = mX * 1;
            Y = mY * -1;
            ScaleX = (float)Math.Sqrt(Math.Pow(m1, 2) + Math.Pow(m2, 2));
            ScaleY = (float)Math.Sqrt(Math.Pow(m3, 2) + Math.Pow(m4, 2));
            det = m1 * m4 - m3 * m2;

            if (det < 0)
            {
                if (IsFirst || prevScaleX < prevScaleY)
                {
                    ScaleX *= -1;
                    IsFirst = false;
                }
                else
                {
                    ScaleY *= -1;
                }
            }

            if (Math.Abs(ScaleX) < 1e-3 || Math.Abs(ScaleY) < 1e-3)
            {
                Angle = prevAngle;
            }
            else
            {
                double sinApx = 0.5 * (m3 / ScaleY - m2 / ScaleX);
                double cosApx = 0.5 * (m1 / ScaleX + m4 / ScaleY);
                Angle = Math.Atan2(sinApx, cosApx);
            }

            if (Math.Abs(Angle - prevAngle) <= Math.PI)
            {
                Spin = -1;
            }
            else
            {
                Spin = 1;
            }

            if (Angle < prevAngle)
            {
                Spin *= -1;
            }

            if (Angle < 0)
            {
               Angle = (float)(Angle + 2 * Math.PI);
            }

            Angle = (float)(Angle * 180 / Math.PI);
        }
    }
}