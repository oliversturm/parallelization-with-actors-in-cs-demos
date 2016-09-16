using System;
using System.Drawing;
using System.Linq;

namespace MandelbrotActors {
  public class PointResult {
    public PointResult(Point point, Color color) {
      Point = point;
      Color = color;
    }
    public Point Point { get; }
    public Color Color { get; }
  }
}