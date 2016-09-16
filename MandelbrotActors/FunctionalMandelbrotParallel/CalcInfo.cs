#region Copyright
/* Copyright 2009-2010 Oliver Sturm <oliver@sturmnet.org> All rights reserved. */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FunctionalMandelbrotParallel {
  public class CalcInfo {
    public CalcInfo(double xStart, double xStep, double yStart, double yStep) {
      XStart = xStart;
      XStep = xStep;
      YStart = yStart;
      YStep = yStep;
    }
    public double XStart { get; private set; }
    public double XStep { get; private set; }
    public double YStart { get; private set; }
    public double YStep { get; private set; }
  }
}
