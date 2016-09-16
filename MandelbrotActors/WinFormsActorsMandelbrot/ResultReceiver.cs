using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Event;
using MandelbrotActors;

namespace WinFormsActorsMandelbrot {
  public class ResultReceiver : TypedActor, IHandle<ResultReceiver.Init>, IHandle<PointResult>, IHandle<AreaCalculator.AreaCalculatorDone> {
    System.Drawing.Point maxPoint = System.Drawing.Point.Empty, minPoint = System.Drawing.Point.Empty;
    int resultCount = 0;

    public void Handle(Init init) {
      //syncContext = init.SyncContext;
      bitmapLock = init.BitmapLock;
      invalidator = init.Invalidator;
      bitmap = init.Bitmap;
      panel = init.Panel;
      maxPoint = new System.Drawing.Point(0, 0);
      minPoint = new System.Drawing.Point(Int32.MaxValue, Int32.MaxValue);

      invalidator.Tell(panel.ClientRectangle);
    }

    public void Handle(PointResult pr) {
      if (bitmap == null) return;

      resultCount++;

      minPoint = new System.Drawing.Point(Math.Min(minPoint.X, pr.Point.X), Math.Min(minPoint.Y, pr.Point.Y));
      maxPoint = new System.Drawing.Point(Math.Max(maxPoint.X, pr.Point.X), Math.Max(maxPoint.Y, pr.Point.Y));

      lock (bitmapLock)
        bitmap.SetPixel(pr.Point.X, pr.Point.Y,
          System.Drawing.Color.FromArgb(pr.Color.R, pr.Color.G, pr.Color.B));
      if (resultCount % 1000 == 0) {
        invalidator.Tell(new Rectangle(minPoint, new Size(maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y)));
        maxPoint = new System.Drawing.Point(0, 0);
        minPoint = new System.Drawing.Point(Int32.MaxValue, Int32.MaxValue);
      }
      //syncContext.Post(o => {
      //  panel.Invalidate();
      //}, null);
    }

    public void Handle(AreaCalculator.AreaCalculatorDone message) {
      invalidator.Tell(panel.ClientRectangle);
    }


    //readonly ILoggingAdapter logger = Logging.GetLogger(Context);
    IActorRef invalidator;

    Bitmap bitmap;
    Panel panel;
    //SynchronizationContext syncContext;
    object bitmapLock;

    public class Init {
      public Init(/*SynchronizationContext syncContext, */object bitmapLock, IActorRef invalidator, Panel panel, Bitmap bitmap, int width, int height) {
        //SyncContext = syncContext;
        BitmapLock = bitmapLock;
        Invalidator = invalidator;
        Panel = panel;
        Bitmap = bitmap;
        Width = width;
        Height = height;
      }

      //public SynchronizationContext SyncContext { get; }
      public object BitmapLock { get; }
      public IActorRef Invalidator { get; }
      public Panel Panel { get; }
      public Bitmap Bitmap { get; }
      public int Width { get; }
      public int Height { get; }
    }

  }
}
