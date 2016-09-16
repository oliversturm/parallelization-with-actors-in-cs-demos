using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Akka.Actor;

namespace WinFormsActorsMandelbrot {
  public class Invalidator : TypedActor, IHandle<Rectangle> {
    public Invalidator(Panel panel) {
      this.panel = panel;
    }
    Panel panel;

    public void Handle(Rectangle message) {
      panel.Invalidate(message);
    }
  }
}
