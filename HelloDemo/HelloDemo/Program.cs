using Akka.Actor;

namespace HelloDemo {
  internal class Program {
    static void Main(string[] args) {
      var system = ActorSystem.Create("olisystem");
      var hello = system.ActorOf<HelloActor>("hello");
      hello.Tell(new HelloActor.Hello("Oli"));
      Console.ReadLine();
    }
  }

  public class HelloActor : UntypedActor {
    protected override void OnReceive(object message) {
      switch (message) {
        case Hello h:
          Console.WriteLine($"Hi {h.Sender}");
          break;
      }
    }

    public record Hello(string Sender);
  }
}
