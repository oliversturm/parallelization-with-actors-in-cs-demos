﻿using Akka.Configuration;
using Akka.Actor;

var config = ConfigurationFactory.ParseString(@"
akka {
    stdout-loglevel = DEBUG
    loglevel = DEBUG
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

using (var system = ActorSystem.Create("EmptyActorService", config)) {
  Console.WriteLine("Empty Actor Service is running");
  Console.ReadLine();
}
