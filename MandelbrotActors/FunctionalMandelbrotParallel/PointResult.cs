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
  public class PointResult {
    public PointResult(Point point, Color color) {
      Point = point;
      Color = color;
    }
    public Point Point { get; private set; }
    public Color Color { get; private set; }
  }
}
