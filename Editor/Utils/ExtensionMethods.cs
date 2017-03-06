using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LevelEditor.Utils
{
    public static class ExtensionMethods
    {
        public static Vector3 AlignedRotation(this Vector3 v, Vector3 dir)
        {
            v = Vector3.TransformVector(v, Matrix4.CreateRotationX((float)Math.Atan2(dir.Y, dir.Z) + (float)Math.PI * 0.5f));
            v = Vector3.TransformVector(v, Matrix4.CreateRotationZ((float)Math.Atan2(dir.Y, dir.X) + (float)Math.PI * 0.5f));
            return v;
        }

        public static Vector3 Floor(this Vector3 v)
        {
            v.X = (float)Math.Floor(v.X);
            v.Y = (float)Math.Floor(v.Y);
            v.Z = (float)Math.Floor(v.Z);
            return v;
        }
        public static Vector3 Round(this Vector3 v)
        {
            v.X = (float)Math.Round(v.X);
            v.Y = (float)Math.Round(v.Y);
            v.Z = (float)Math.Round(v.Z);
            return v;
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static string CamelCaseToHuman(this string value)
        {
            return Regex.Replace(value, "(?!^)([A-Z0-9])", " $1");
        }

        public static byte[] ToBytesFractional(this float value)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)Math.Min(255f, Math.Floor(value));
            bytes[0] = (byte)Math.Min(255f, 255f * (value - bytes[1]));
            return bytes;
        }
    }
}
