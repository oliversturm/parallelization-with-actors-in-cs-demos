using Akka.Actor;
using MandelbrotActors;

namespace WinFormsActorsMandelbrotNew {
  public class ResultReceiver : UntypedActor {
    System.Drawing.Point maxPoint = System.Drawing.Point.Empty, minPoint = System.Drawing.Point.Empty;
    int resultCount = 0;

    protected override void OnReceive(object message) {
      switch (message) {
        case Init i:
          Handle(i); break;
        case PointResult pr:
          Handle(pr); break;
        case AreaCalculator.AreaCalculatorDone acd:
          Handle(acd); break;
      }
    }

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
      if (bitmapLock == null) return;

      resultCount++;

      minPoint = new System.Drawing.Point(Math.Min(minPoint.X, pr.Point.X), Math.Min(minPoint.Y, pr.Point.Y));
      maxPoint = new System.Drawing.Point(Math.Max(maxPoint.X, pr.Point.X), Math.Max(maxPoint.Y, pr.Point.Y));

      lock (bitmapLock)
        bitmap.SetPixel(pr.Point.X, pr.Point.Y,
          System.Drawing.Color.FromArgb(pr.Color.R, pr.Color.G, pr.Color.B));
      if (resultCount % 10000 == 0) {
        invalidator.Tell(new Rectangle(minPoint, new Size(maxPoint.X - minPoint.X, maxPoint.Y - minPoint.Y)));
        maxPoint = new System.Drawing.Point(0, 0);
        minPoint = new System.Drawing.Point(Int32.MaxValue, Int32.MaxValue);
      }
      //syncContext?.Post(o => {
      //  panel.Invalidate();
      //}, null);
    }

    public void Handle(AreaCalculator.AreaCalculatorDone message) {
      if (panel == null) return;
      invalidator.Tell(panel.ClientRectangle);
    }


    //readonly ILoggingAdapter logger = Logging.GetLogger(Context);
    IActorRef? invalidator;

    Bitmap? bitmap;
    Panel? panel;
    //SynchronizationContext? syncContext;
    object? bitmapLock;

    public record Init(/*SynchronizationContext syncContext, */object BitmapLock, IActorRef Invalidator, Panel Panel, Bitmap Bitmap, int Width, int Height);
  }
}
