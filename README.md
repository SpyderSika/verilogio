# verilogio

## summary
generate text diagram comment from verilog module declaration.<p>
at this version, support verilog 1995/2001 only. System Verilog will support in the future.
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

## How to use
It's console app. execute app and supply option and verilog source as following:

Windwos:
verilogio.exe -d targetmodule.v

Linux:(not tested)
verilogio -d targetmodule.v

verilogio.exe shows result at console.

test is the name of module.
each input signals are shown at left side.
output or inout signals are shown at right side.
'->' means arrow and signal direction.
if the signal is the bus, '=>' is used for arrow.

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

## License
This library is provided under MIT License.

## Author
I don't open my real name, so SpyderSika is the name at github.
