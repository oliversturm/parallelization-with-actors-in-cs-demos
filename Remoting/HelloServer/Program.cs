using System;
using Akka.Actor;
using Akka.Configuration;
using RemoteHelloMessages;

namespace HelloServer {
  class MainClass {
    public static void Main(string [] args) {
      var config = ConfigurationFactory.ParseString(@"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
            transport-protocol = tcp
            port = 8081
            hostname = 127.0.0.1
        }
    }
}
");

      using (var system = ActorSystem.Create("HelloServer", config)) {
        system.ActorOf<HelloActor>("HelloActor");
        Console.WriteLine("Hello actor is available");
        Console.ReadLine();
      }
    }
  }

  public class HelloActor : ReceiveActor {
    public HelloActor() {
      Receive<Hello>(msg => {
        Console.WriteLine($"Received hello from {msg.SenderName}");
      });
    }
  }

}
