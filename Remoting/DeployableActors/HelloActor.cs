using Akka.Actor;

namespace DeployableActors;

public class HelloActor : UntypedActor {
  public record Hello(string SenderName);

  protected override void OnReceive(object message) {
    switch (message) {
      case Hello h:
        Console.WriteLine($"Received hello from {h.SenderName}");
        break;
    }
  }
}
