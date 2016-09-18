using System;
using Akka.Actor;
using Akka.Configuration;
using RemoteHelloMessages;

namespace HelloClient {
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
        port = 0
        hostname = 127.0.0.1
        }
    }
}
");

      using (var system = ActorSystem.Create("client", config)) {
        var server = system.ActorSelection("akka.tcp://HelloServer@127.0.0.1:8081/user/HelloActor");

        while(true) {
          Console.WriteLine("Please enter your name: ");
          var name = Console.ReadLine();
          server.Tell(new Hello(name));
        }

      }

    }
  }
}
