using System;
using System.Windows;

namespace Sample.WpfNeko
{
    /// <summary>
    /// 数学ユーティリティ
    /// </summary>
    public static class MathUtil
    {
        private static readonly Random _random = new Random();

        public static double DegreeToRadian(double degree)
        {
            return degree / 180.0 * Math.PI;
        }

        public static double RadianToDegree(double radian)
        {
            return radian / Math.PI * 180.0;
        }

        public static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public static int Rand(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static int Rand(int maxValue)
        {
            return _random.Next(maxValue);
        }
    }
}
