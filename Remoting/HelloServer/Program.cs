using Akka.Actor;
using Akka.Configuration;

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
