using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;

namespace MandelbrotActors {
  public class PointCalculator : TypedActor, IHandle<PointCalculator.CalcPoint> {
    static PointCalculator() {
      InitColors();
    }

    const int COLOR_COUNT = 10;
    private static void InitColors() {
      colors = new Color[COLOR_COUNT];
      colors[0] = Color.White;
      colors[1] = new Color(0, 0, 25);
      colors[2] = new Color(0, 0, 50);
      colors[3] = new Color(0, 0, 75);
      colors[4] = new Color(0, 0, 100);
      colors[5] = new Color(0, 0, 125);
      colors[6] = new Color(0, 0, 150);
      colors[7] = new Color(0, 0, 175);
      colors[8] = new Color(0, 0, 200);
      colors[9] = new Color(0, 0, 255);
    }
    static Color[] colors;

    public void Handle(CalcPoint message) {
      var iterations = Iterator(
        message.CalcInfo.XStart + message.Point.X * message.CalcInfo.XStep,
        message.CalcInfo.YStart + message.Point.Y * message.CalcInfo.YStep,
        message.CalcInfo.MaxIterations);
      message.ResultReceiver.Tell(
        new PointResult(message.Point, ColorFromIterations(iterations, message.CalcInfo.MaxIterations)));
      Sender.Tell(new AreaCalculator.PointDone());
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