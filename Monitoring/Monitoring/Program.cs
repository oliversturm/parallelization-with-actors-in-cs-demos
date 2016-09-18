using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring {
  class Program {
    static void Main(string[] args) {
      var system = ActorSystem.Create("Calculator");
      var calculator = system.ActorOf<Calculator>("Calc");
      calculator.Tell(new Calculator.Calculate(Calculator.Calculation.Division, 10, 2));
      calculator.Tell(new Calculator.Calculate(Calculator.Calculation.Division, 10, 0));
      calculator.Tell(new Calculator.Calculate(Calculator.Calculation.Division, 15, 0));
      Console.ReadLine();
    }

    public class Calculator : ReceiveActor {
      public Calculator() {
        divider = Context.ActorOf<DivideActor>();

        Receive<DivideActor.DivideResult>(r => {
          Console.WriteLine($"Received divide result: {r.Result}");
        });

        Receive<Calculate>(c => {
          if (c.Operation == Calculation.Division)
            divider.Tell(new DivideActor.Divide(c.Op1, c.Op2));
        });
      }
      IActorRef divider;

      protected override SupervisorStrategy SupervisorStrategy() {
        return new OneForOneStrategy( 
          // we can specify maximum attempts for restarting, within certain timeframes
            //maxNrOfRetries: 10,
            //withinTimeRange: TimeSpan.FromSeconds(30),

            decider: Decider.From(ex => {
              if (ex is DivideByZeroException)
                // this lets the actor resume with its existing internal state
                return Directive.Resume;
                // alternatively, restart it, thereby resetting its state
                //return Directive.Restart;

              else 
                return Directive.Stop;
            }));
      }

      public enum Calculation {
        Division
      }

      public class Calculate {
        public Calculate(Calculation operation, int op1, int op2) {
          Operation = operation;
          Op1 = op1;
          Op2 = op2;
        }
        public Calculation Operation { get; }
        public int Op1 { get; set; }
        public int Op2 { get; set; }
      }
    }

    public class DivideActor : ReceiveActor {
      public DivideActor() {
        Receive<Divide>(m => {
          Console.WriteLine($"Dividing {m.Dividend} by {m.Divisor}");
          Sender.Tell(new DivideResult(m.Dividend / m.Divisor));
        });
      }

      public class Divide {
        public Divide(int dividend, int divisor) {
          Dividend = dividend;
          Divisor = divisor;
        }
        public int Dividend { get; }
        public int Divisor { get; }
      }

      public class DivideResult {
        public DivideResult(int result) {
          Result = result;
        }
        public int Result { get; }
      }
    }

  }
}
