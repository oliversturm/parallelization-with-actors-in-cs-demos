#region Copyright
/* Copyright 2009-2010 Oliver Sturm <oliver@sturmnet.org> All rights reserved. */
#endregion

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FunctionalMandelbrotParallel;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AutomaticConcurrent {
  public partial class Form1 : Form {
    public Form1( ) {
      InitializeComponent( );

      syncContext = SynchronizationContext.Current;
    }

    SynchronizationContext syncContext;

    const double xstart = -2.1;
    const double xend = 1.0;
    const double ystart = -1.3;
    const double yend = 1.3;

    private void DrawButton_Click(object sender, EventArgs e) {
      Calculator.MaxIteration = Convert.ToInt32(IterationsUD.Value);

      int height = panel.Height;
      int width = panel.Width;
      paintImage = new Bitmap(width, height);
      double xstep = (xend - xstart) / width;
      double ystep = (yend - ystart) / height;

      resultQueue = new ConcurrentQueue<PointResult>( );
      var uiCancel = new CancellationTokenSource( );
      Task uiUpdateTask = Task.Factory.StartNew(( ) => UIUpdate(uiCancel.Token), uiCancel.Token);

      var calcInfo = new CalcInfo(xstart, xstep, ystart, ystep);
      Task mainTask = Task.Factory.StartNew(( ) =>
        Calculator.CalcArea(panel.Width, panel.Height, calcInfo, AcceptResult));
      mainTask.ContinueWith(t => uiCancel.Cancel( ));
    }

    const int UPDATE_THRESHOLD = 10000;

    private void UIUpdateFromResults(int count) {
      Point maxPoint = new Point(0, 0), minPoint = new Point(Int32.MaxValue, Int32.MaxValue);
      lock(paintImageLock) {
        for (int i = 0; i < count; i++) {
          PointResult pointResult;

          if (resultQueue.TryDequeue(out pointResult)) {
            paintImage.SetPixel(pointResult.Point.X, pointResult.Point.Y, pointResult.Color);
            minPoint = new Point(Math.Min(minPoint.X, pointResult.Point.X), Math.Min(minPoint.Y, pointResult.Point.Y));
            maxPoint = new Point(Math.Max(maxPoint.X, pointResult.Point.X), Math.Max(maxPoint.Y, pointResult.Point.Y));
          }
        }
      }
      syncContext.Post(o => {
        panel.Invalidate(new Rectangle(minPoint, new Size(maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y)));
      }, null);
    }

    void UIUpdate(CancellationToken token) {
      while (!(token.IsCancellationRequested)) {
        if (resultQueue.Count > UPDATE_THRESHOLD)
          UIUpdateFromResults(UPDATE_THRESHOLD);
        else
          Thread.Sleep(50);
      }
      UIUpdateFromResults(resultQueue.Count);
    }

    void AcceptResult(PointResult pointResult) {
      resultQueue.Enqueue(pointResult);
    }

    ConcurrentQueue<PointResult> resultQueue;
    Bitmap paintImage;
    object paintImageLock = new object( );

    private void ClearButton_Click(object sender, EventArgs e) {
      paintImage = null;
      panel.Invalidate( );
    }

    private void panel_Paint(object sender, PaintEventArgs e) {
      if (paintImage != null) {
        lock(paintImageLock)
          e.Graphics.DrawImage(paintImage, 0, 0);
      }
    }
  }
}