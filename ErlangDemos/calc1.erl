-module(calc1).

-export([loop/0]).

loop() ->
    receive
        % Very simple format message, using an atom to identify the operation
        {add, A, B} -> 
            io:format("adding: ~p~n", [A + B]),
            loop();
        {mult, A, B} -> 
            io:format("multiplying: ~p~n", [A * B]),
            loop()
    end.