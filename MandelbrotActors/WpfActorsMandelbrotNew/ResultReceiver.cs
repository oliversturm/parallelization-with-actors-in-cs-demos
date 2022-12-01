using Akka.Actor;
using MandelbrotActors;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfActorsMandelbrotNew {
  public class ResultReceiver : UntypedActor {
    protected override void OnReceive(object message) {
      switch (message) {
        case Init init:
          wb = BitmapFactory.New(init.Width, init.Height);
          init.Image.Source = wb;
          break;

        case PointResult pr:
          if (wb == null) return;
          wb.SetPixel(pr.Point.X, pr.Point.Y,
            pr.Color.R, pr.Color.G, pr.Color.B);
          break;
      }
    }

    WriteableBitmap? wb;

    public class Init {
      public Init(Image image, int width, int height) {
        Image = image;
        Width = width;
        Height = height;
      }

      public Image Image { get; }
      public int Width { get; }
      public int Height { get; }
    }
  }
}
