using Akka.Actor;
using Akka.Configuration;
using MandelbrotActors;

namespace WinFormsActorsMandelbrotNew {
  public partial class Form1 : Form {
    public Form1() {
      InitializeComponent();
    }

    ActorSystem? actorSystem;
    public ActorSystem MandelbrotActors =>
  actorSystem ??= ActorSystem.Create("MandelbrotSystem", ConfigurationFactory.ParseString(@"
synchronized-dispatcher {
  type = ""SynchronizedDispatcher""
  throughput = 10
}
")
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
    );

    IActorRef? areaCalculator;
    public IActorRef AreaCalculator =>
        areaCalculator ??= MandelbrotActors.ActorOf(
            Props.Create<AreaCalculator>(), "areaCalculator");

    IActorRef? resultReceiver;
    public IActorRef ResultReceiver =>
        resultReceiver ??= MandelbrotActors.ActorOf(
            Props.Create<ResultReceiver>(), "resultReceiver");

    IActorRef? invalidator;
    public IActorRef Invalidator =>
        invalidator = MandelbrotActors.ActorOf(
            Props.Create<Invalidator>(panel).WithDispatcher("synchronized-dispatcher"),
            "invalidator");

    Bitmap? bitmap;
    object bitmapLock = new object();


    private void drawButton_Click(object sender, EventArgs e) {
      int imageWidth = (int)panel.Width;
      int imageHeight = (int)panel.Height;
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
        lock (bitmapLock)
          e.Graphics.DrawImage(bitmap, 0, 0);
      }
    }
  }
}