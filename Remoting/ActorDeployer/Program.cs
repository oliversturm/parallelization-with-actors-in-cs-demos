using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Configuration;
using DeployableActors;

namespace ActorDeployer
{
  class Program
  {
    static void Main(string[] args)
    {
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

      using (var system = ActorSystem.Create("client", config))
      {
        var helloActor = system.ActorOf(Props.Create(() => new HelloActor()), "DeployedHello");

        while (true)
        {
          Console.WriteLine("Please enter your name: ");
          var name = Console.ReadLine();
          helloActor.Tell(new Hello(name));
        }

      }


    }
  }
}


