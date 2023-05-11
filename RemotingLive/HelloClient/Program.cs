using Akka.Actor;
using Akka.Configuration;

using HelloMessages;

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

Console.WriteLine("Press RETURN to start the client");
Console.ReadLine();

