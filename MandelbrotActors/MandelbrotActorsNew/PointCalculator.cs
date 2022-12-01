using Akka.Actor;

namespace MandelbrotActors {
  public class PointCalculator : UntypedActor {
    static PointCalculator() {
    }

    const int COLOR_COUNT = 10;
    static Color[] colors = new Color[] {
      Color.White,
      new Color(0, 0, 25),
      new Color(0, 0, 50),
      new Color(0, 0, 75),
      new Color(0, 0, 100),
      new Color(0, 0, 125),
      new Color(0, 0, 150),
      new Color(0, 0, 175),
      new Color(0, 0, 200),
      new Color(0, 0, 255)
    };

    protected override void OnReceive(object message) {
      switch (message) {
        case CalcPoint cp:
          var iterations = Iterator(
            cp.CalcInfo.XStart + cp.Point.X * cp.CalcInfo.XStep,
            cp.CalcInfo.YStart + cp.Point.Y * cp.CalcInfo.YStep,
            cp.CalcInfo.MaxIterations);
          cp.ResultReceiver.Tell(
            new PointResult(cp.Point, ColorFromIterations(iterations, cp.CalcInfo.MaxIterations)));
          Sender.Tell(new AreaCalculator.PointDone());
          break;
      }
    }

    Color ColorFromIterations(int iterations, int maxIterations) {
      return iterations == maxIterations ?
        Color.Black : colors[iterations % COLOR_COUNT];
    }

    int Iterator(double x, double y, int maxIterations) {
      double tx = 0, ty = 0;
      int iteration = 0;
      while ((tx * tx + ty * ty) < (2 * 2) &&
        iteration < maxIterations) {
        double ttx = tx * tx - ty * ty + x;
        ty = 2 * tx * ty + y;
        tx = ttx;
        iteration++;
      }
      return iteration;
    }

    public class CalcPoint {
      public CalcPoint(IActorRef resultReceiver, Point point, CalcInfo calcInfo) {
        ResultReceiver = resultReceiver;
        Point = point;
        CalcInfo = calcInfo;
      }

      public IActorRef ResultReceiver { get; }
      public Point Point { get; }
      public CalcInfo CalcInfo { get; }
    }
  }
}