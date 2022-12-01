using Akka.Actor;
using Akka.Configuration;
using MandelbrotActors;
using System.Windows;

namespace WpfActorsMandelbrotNew {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
    }

    ActorSystem? actorSystem;
    public ActorSystem MandelbrotActors => actorSystem ??=
      ActorSystem.Create("MandelbrotSystem", ConfigurationFactory.ParseString(@"
synchronized-dispatcher {
  type = ""SynchronizedDispatcher""
  throughput = 10
}
"));

    IActorRef? areaCalculator;
    public IActorRef AreaCalculator => areaCalculator ??=
      MandelbrotActors.ActorOf(Props.Create<AreaCalculator>(), "areaCalculator");

    IActorRef? resultReceiver;
    public IActorRef ResultReceiver => resultReceiver ??=
      MandelbrotActors.ActorOf(
            Props.Create<ResultReceiver>().WithDispatcher("synchronized-dispatcher"),
            "resultReceiver");

    private void drawButton_Click(object sender, RoutedEventArgs e) {
      int imageWidth = (int)imagePanel.ActualWidth;
      int imageHeight = (int)imagePanel.ActualHeight;

      ResultReceiver.Tell(new ResultReceiver.Init(image, imageWidth, imageHeight));

      const double xstart = -2.1;
      const double xend = 1.0;
      const double ystart = -1.3;
      const double yend = 1.3;

      double xstep = (xend - xstart) / imageWidth;
      double ystep = (yend - ystart) / imageHeight;

      AreaCalculator.Tell(new AreaCalculator.CalcArea(ResultReceiver,
        imageWidth, imageHeight,
        new CalcInfo(xstart, xstep, ystart, ystep, 1000)));
    }

  }
}
