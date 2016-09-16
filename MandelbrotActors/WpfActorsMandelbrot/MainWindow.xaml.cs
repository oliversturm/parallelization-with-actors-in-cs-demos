using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Akka.Actor;
using Akka.Configuration;
using MandelbrotActors;

namespace WpfActorsMandelbrot {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
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
            Props.Create<ResultReceiver>().WithDispatcher("synchronized-dispatcher"),
            "resultReceiver");
        }
        return resultReceiver;
      }
    }

    private void drawButton_Click(object sender, RoutedEventArgs e) {
      int imageWidth = (int) imagePanel.ActualWidth;
      int imageHeight = (int) imagePanel.ActualHeight;

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
