#!/bin/bash

docker run --rm -it --volume .:/src -w /src oliversturm/dotnet-demo:1 bash -c "tmux new-session -d -s demo 'dotnet run --project HelloServer --no-restore'; tmux split-window -h -t demo 'dotnet run --project HelloClient --no-restore'; tmux attach-session -t demo"
