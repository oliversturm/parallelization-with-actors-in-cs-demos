using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Akka.Actor;
using Akka.Configuration;
using Akka.Event;
using MandelbrotActors;

namespace WinFormsActorsMandelbrot {
  public partial class Form1 : Form {
    public Form1() {
      InitializeComponent();
    }

    ActorSystem actorSystem;
    public ActorSystem MandelbrotActors {
      get {
        if (actorSystem == null) {
          var config = ConfigurationFactory.ParseString(@"
synchronized-dispatcher {
  type = ""SynchronizedDispatcher""
  throughput = 10
}
");
//task-dispatcher {
//  type = ""TaskDispatcher""
//  throughput = 100
//}
//");

//akka {  
//    stdout-loglevel = DEBUG
//    loglevel = DEBUG
//    log-config-on-start = on        
//    actor {                
//        debug {  
//              receive = on 
//              autoreceive = on
//              lifecycle = on
//              event-stream = on
//              unhandled = on
//        }
//    }
//");
          actorSystem = ActorSystem.Create("MandelbrotSystem", config);
        }

        return actorSystem;
      }
    }

    IActorRef areaCalculator;
    public IActorRef AreaCalculator {
      get {
        if (areaCalculator == null) {
          areaCalculator = MandelbrotActors.ActorOf(Props.Create<AreaCalculator>(), "areaCalculator");
        }
        return areaCalculator;
      }
    }

    IActorRef resultReceiver;
    public IActorRef ResultReceiver {
      get {
        if (resultReceiver == null) {
          resultReceiver = MandelbrotActors.ActorOf(
            Props.Create<ResultReceiver>(),
            "resultReceiver");
        }
        return resultReceiver;
      }
    }

    IActorRef invalidator;
    public IActorRef Invalidator {
      get {
        if (invalidator == null) {
          invalidator = MandelbrotActors.ActorOf(
            Props.Create<Invalidator>(panel).WithDispatcher("synchronized-dispatcher"),
            "invalidator");
        }
        return invalidator;
      }
    }
    Bitmap bitmap;
    object bitmapLock = new object();

    private void drawButton_Click(object sender, EventArgs e) {
      int imageWidth = (int) panel.Width;
      int imageHeight = (int) panel.Height;
      bitmap = new Bitmap(imageWidth, imageHeight);

      ResultReceiver.Tell(new ResultReceiver.Init(/*SynchronizationContext.Current, */bitmapLock, Invalidator, panel, bitmap, imageWidth, imageHeight));

      const double xstart = -2.1;
      const double xend = 1.0;
      const double ystart = -1.3;
      const double yend = 1.3;

      double xstep = (xend - xstart) / imageWidth;
      double ystep = (yend - ystart) / imageHeight;

      AreaCalculator.Tell(new AreaCalculator.CalcArea(ResultReceiver,
        imageWidth, imageHeight,
        new CalcInfo(xstart, xstep, ystart, ystep, 10000)));
    }

    private void panel_Paint(object sender, PaintEventArgs e) {
      if (bitmap != null) {
        lock(bitmapLock)
          e.Graphics.DrawImage(bitmap, 0, 0);
      }
    }
  }
}
