using System;
using Akka.Actor;

namespace DeployableActors
{
  public class HelloActor : ReceiveActor
  {
    public HelloActor()
    {
      Receive<Hello>(msg => {
        Console.WriteLine($"Received hello from {msg.SenderName}");
      });
    }
  }

  public class Hello
  {
    public Hello(string senderName)
    {
      SenderName = senderName;
    }

    public string SenderName { get; }
  }
}



