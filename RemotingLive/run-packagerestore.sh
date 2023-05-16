#!/bin/bash

# complicated bash nonsense to mimick my usual (zsh) `rm -rf */(bin|obj) packages`
echo 'Removing `bin`, `obj` and `packages` folders'
shopt -s extglob
rm -rf */@(bin|obj) packages

docker run --rm -it --volume .:/src -w /src oliversturm/dotnet-demo:1 bash -c 'for p in HelloServer HelloClient; do echo Project $p;  dotnet restore $p --packages ./packages; done'
