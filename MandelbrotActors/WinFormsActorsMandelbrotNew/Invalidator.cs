using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Akka.Actor;

namespace WinFormsActorsMandelbrotNew {
  public class Invalidator : UntypedActor {
    public Invalidator(Panel panel) {
      this.panel = panel;
    }
    Panel panel;

    protected override void OnReceive(object message) {
      switch (message) {
        case Rectangle r:
          panel.Invalidate(r);
          break;
      }
    }
  }
}
