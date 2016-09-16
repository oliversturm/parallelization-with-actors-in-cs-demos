using System;
using System.Linq;

namespace MandelbrotActors {
  public class CalcInfo {
    public CalcInfo(double xStart, double xStep, double yStart, double yStep, int maxIterations) {
      XStart = xStart;
      XStep = xStep;
      YStart = yStart;
      YStep = yStep;
      MaxIterations = maxIterations;
    }

    public double XStart { get; }
    public double XStep { get; }
    public double YStart { get; }
    public double YStep { get; }
    public int MaxIterations { get; }
  }
}