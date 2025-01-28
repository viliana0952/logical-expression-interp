# logical-expression-interp
  Task: A program that allows working with logical expressions. The program gets its parameters from the command line. It allows building and using a collection of logical functions.
   
  The program has the following functionalities:
  1. Entering logic functions
      Each function is defined as a named logical expression composed of the basic logical AND, OR, and NOT operations or other user-entered functions. The input of a logical function must check for errors. The base operations must have a predefined priority, which can be changed by the use of parentheses.

     Example: DEFINE func1(a, b): 'a & b'

  3. Write in and read from file
      Write (when defining a new function) and read (when starting the program) the user-defined functions from a file.

  4. Solving a function for given parameters

     Examples:
     
   	    SOLVE func1(1, 0) -> Result: 0;
     
   	    SOLVE func2(1, 0, 1) -> Result: 1;

  6. Produce a truth table for a logical function
     
     Example:

                  ALL func1 -> a | b | res
                               0 | 0 | 0
                               0 | 1 | 0
                               1 | 0 | 0
                               1 | 1 | 1

  8. Finding a logic function via a truth table
     
      Example:

         FIND 0,0,0:0;
              0,0,1:0;
              0,1,0:0;
              0,1,1:0;
              1,0,0:0;
              1,0,1:0;
              1,1,0:0;
              1,1,1:1
