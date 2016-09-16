-module(calc3).

-export([start/0, add/2, mult/2]).

start() ->
    % Register the service instead of returning it
    register(calcservice, spawn(fun loop/0)).
        
add(A, B) ->
    % I find the calc service that was previously registered, so the caller
    % doesn't have to tell me about it anymore.
    % Could do this if I wanted the variable:
    % Calc = whereis(calcservice),
    % Calc ! {self(), add, A, B},

    % but I can also send to the atom directly
    calcservice ! {self(), add, A, B},
    receive
        {result, Result} -> Result
    end.

mult(A, B) ->
    calcservice ! {self(), mult, A, B},
    receive
        {result, Result} -> Result
    end.


loop() ->
    receive
        {Sender, add, A, B} -> 
            Result = A + B,
            io:format("adding: ~p~n", [Result]),
            Sender ! {result, Result},
            loop();
        {Sender, mult, A, B} -> 
            Result = A * B,
            io:format("multiplying: ~p~n", [Result]),
            Sender ! {result, Result},
            loop();
        Other -> 
            % I catch this case here - the process is now publicly registered,
            % so it's possible that I receive unknown messages.
            io:format("I don't know how to do ~p~n", [Other]),
            loop()
    end.
