-module(calc2).

-export([start/0, add/3, mult/3]).


start() ->
    spawn(fun loop/0).
        
add(Calc, A, B) ->
    % Let the Calc process know I want it to do something
    % I give it a reference to myself so it can send the result in a new message
    Calc ! {self(), add, A, B},
    receive
        % This is the result message. Through the Calc part of the tuple it's matched
        % against the process I sent a message to myself. Of course this could be 
        % extended in case that match is not "safe" enough.
        {result, Result} -> Result
    end.

mult(Calc, A, B) ->
    Calc ! {self(), mult, A, B},
    receive
        {result, Result} -> Result
    end.


loop() ->
    receive
        % I get the Sender of the message passed in - this is often called From instead of Sender.
        {Sender, add, A, B} -> 
            Result = A + B,
            io:format("adding: ~p~n", [Result]),
            % Send a message back to the sender with the result.
            % I also include my own ID, so the sender can distinguish my result message from others.
            Sender ! {result, Result},
            % And I'm off to do it again.
            loop();
        {Sender, mult, A, B} -> 
            Result = A * B,
            io:format("multiplying: ~p~n", [Result]),
            Sender ! {result, Result},
            loop()
    end.
