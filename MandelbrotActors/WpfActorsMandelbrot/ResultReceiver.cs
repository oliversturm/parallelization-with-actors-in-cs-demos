using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Akka.Actor;
using MandelbrotActors;

namespace WpfActorsMandelbrot {
  public class ResultReceiver : ReceiveActor {
    public ResultReceiver() {
      Receive<Init>(init => {
        wb = BitmapFactory.New(init.Width, init.Height);
        init.Image.Source = wb;
      });
      Receive<PointResult>(pr => {
        if (wb == null) return;
        wb.SetPixel(pr.Point.X, pr.Point.Y,
          pr.Color.R, pr.Color.G, pr.Color.B);
      });
    }

    WriteableBitmap wb;

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
