using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;

namespace MandelbrotActors {
  public class AreaCalculator : ReceiveActor {
    public AreaCalculator() {
      pointCalculator = Context.ActorOf<PointCalculator>("pointCalculator");
      // equivalent to 
      // Context.ActorOf(Props.Create<CalcPointActor>(), "calcPoint");

      Become(Free);
    }

    IActorRef pointCalculator;
    int pointCount = 0;
    IActorRef resultReceiver;

    public void Free() {
      Receive<CalcArea>(ca => {
        var points = PointSequence(ca.Width, ca.Height);

        foreach (var p in points) {
          pointCount++;
          pointCalculator.Tell(new PointCalculator.CalcPoint(
            ca.ResultReceiver, p, ca.CalcInfo));
        }

        resultReceiver = ca.ResultReceiver;
        Become(Busy);
      });
    }

    public void Busy() {
      Receive<PointDone>(pd => {
        if (--pointCount == 0) {
          resultReceiver.Tell(new AreaCalculatorDone());
          Become(Free);
        }
      });
    }

    public static IEnumerable<Point> PointSequence(int width, int height) {
      for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
          yield return new Point(x, y);
    }
  
    public class AreaCalculatorDone { }
    public class PointDone { }

    public class CalcArea {
      public CalcArea(IActorRef resultReceiver, int width, int height, CalcInfo calcInfo) {
        ResultReceiver = resultReceiver;
        Height = height;
        Width = width;
        CalcInfo = calcInfo;
      }

      public IActorRef ResultReceiver { get; }
      public int Width { get; }
      public int Height { get; }
      public CalcInfo CalcInfo { get; }
    }
  }
}