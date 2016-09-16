using System;
using System.Linq;

namespace MandelbrotActors {
  public struct Point {
    public Point(int x, int y) {
      X = x;
      Y = y;
    }

    public static readonly Point Empty = new Point();
    public int X { get; }
    public int Y { get; }
  }
}