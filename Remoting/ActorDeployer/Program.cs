using Akka.Configuration;
using Akka.Actor;

using DeployableActors;

var config = ConfigurationFactory.ParseString(@"
  akka {
      actor{
          provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
          deployment {
              /DeployedHello {
                  remote = ""akka.tcp://EmptyActorService@127.0.0.1:8081""
              }
          }
      }
      remote {
          helios.tcp {
              transport-protocol = tcp
              port = 0
              hostname = localhost
          }
      }
  }");

Console.WriteLine("Press RETURN to start the client");
Console.ReadLine();

using (var system = ActorSystem.Create("client", config)) {
  var helloActor = system.ActorOf(Props.Create(() => new HelloActor()), "DeployedHello");

  while (true) {
    Console.WriteLine("Please enter your name: ");
    var name = Console.ReadLine();
    helloActor.Tell(new HelloActor.Hello(name ?? "no name"));
  }
}
