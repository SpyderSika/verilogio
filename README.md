# verilogio

generate comment from verilog module input/output/inout
note: at this moment, not tested and just implemented as class, not executable.

if read verilog code,
```C
module test(
input clk;
input reset_b;

input a;
input [3:0] b; // bbb

output add;
);

endmodule
```

result is
```c
//test
//         ################
//clk    ->#              #
//reset_b->#              #
//a      ->#              #
//b[3:0] =>#              #
//         #              #->add
//         ################
```
