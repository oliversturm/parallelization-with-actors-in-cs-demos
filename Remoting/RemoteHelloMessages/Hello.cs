using System;
namespace RemoteHelloMessages {
  public class Hello {
    public Hello(string senderName) {
      SenderName = senderName;
    }
    public string SenderName { get; }
  }
}

