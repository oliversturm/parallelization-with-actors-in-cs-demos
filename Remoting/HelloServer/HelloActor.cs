using Akka.Actor;
using HelloMessages;

public class HelloActor : UntypedActor {
  protected override void OnReceive(object message) {
    switch (message) {
      case Hello h:
        Console.WriteLine($"Received hello from {h.SenderName}");
        break;
    }
  }
}

