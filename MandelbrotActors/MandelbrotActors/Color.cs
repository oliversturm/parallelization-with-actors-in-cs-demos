using System;
using System.Linq;

namespace MandelbrotActors {
  public struct Color {
    public Color(byte r, byte g, byte b) {
      R = r;
      G = g;
      B = b;
    }

    public static readonly Color Empty = new Color();

    public static readonly Color White = new Color(255, 255, 255);
    public static readonly Color Black = new Color(0, 0, 0);

    public byte R { get; }
    public byte G { get; }
    public byte B { get; }
  }
}