# Machine Learning

## Assignment 1 - Decision tree

## Assignment 2 - Perceptron

There should be an executable at the top level of the zip and a data folder. To run with the default parameters,
just run the executable without and parameters. This will use the packaged train and test set and a learning rate of 0.3
and run the learning algorithm over 400 iterations.

If you want to specify any other arguments you have to specify all of them like this.

`mlAssignment.exe <path to train> <path to test> <learning rate> <number of iterations>`

Example:
`mlAssignment.exe train.dat test.dat 0.5 650`

## Assignmemnt 3 - Markov

There should be an executable at the top level of the zip and a data folder. There are no default parameters. To run, run like this.

`mlAssignment3.exe <number of states> <possible number of actions> <state input file> <discount factor>`

Example:
`mlAssignment.exe 4 2 test.in 0.9`