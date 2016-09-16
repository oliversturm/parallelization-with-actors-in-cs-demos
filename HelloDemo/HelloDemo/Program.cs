using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloDemo {
  // immutable class for the message
  public class Hello {
    public Hello(string sender) {
      Sender = sender;
    }
    public string Sender { get; }
  }

  public class HelloActor : ReceiveActor {
    public HelloActor() {
      Receive<Hello>(message =>
                     Console.WriteLine("Hi {0}", message.Sender));
    }
  }

  class MainClass {
    public static void Main(string[] args) {
      var system = ActorSystem.Create("olisystem");
      // just get hold of the IActorRef directly
      var hello = system.ActorOf<HelloActor>("hello");

      // Or make sure that the actor exists in the system and then query it
      // (note that it's not a recommendation to be doing this all the time,
      // due to the overhead - if the IActorRef is available already, or in 
      // an easier way, use it)
      //system.ActorOf<HelloActor>("hello");
      //var hello = system.ActorSelection("/user/hello");
      hello.Tell(new Hello("Oli"));
      Console.ReadLine();
    }
  }
}
