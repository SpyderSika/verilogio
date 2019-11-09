# verilogio

## summary
generate text diagram comment from verilog module declaration.<p>
at this version, support verilog 1995/2001 only. System Verilog will support in the future.
if read verilog code,
  
note: At this time Not tested well.
  
```C
module test(
input clk,
input reset_b,

input a,
input [3:0] b, // bbb

output add,
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


https://github.com/lowRISC/ibex/tree/master/rtl
ibex_core.sv result is:
```c
//ibex_core
//                      ################
//clk_i               ->#              #
//rst_ni              ->#              #
//test_en_i           ->#              #
//hart_id_i[31:0]     =>#              #
//boot_addr_i[31:0]   =>#              #
//                      #              #->instr_req_o
//instr_gnt_i         ->#              #
//instr_rvalid_i      ->#              #
//                      #              #=>instr_addr_o[31:0]
//instr_rdata_i[31:0] =>#              #
//instr_err_i         ->#              #
//                      #              #->data_req_o
//data_gnt_i          ->#              #
//data_rvalid_i       ->#              #
//                      #              #->data_we_o
//                      #              #=>data_be_o[3:0]
//                      #              #=>data_addr_o[31:0]
//                      #              #=>data_wdata_o[31:0]
//data_rdata_i[31:0]  =>#              #
//data_err_i          ->#              #
//irq_software_i      ->#              #
//irq_timer_i         ->#              #
//irq_external_i      ->#              #
//irq_fast_i[14:0]    =>#              #
//irq_nm_i            ->#              #
//debug_req_i         ->#              #
//                      #              #->rvfi_valid
//                      #              #=>rvfi_order[63:0]
//                      #              #=>rvfi_insn[31:0]
//                      #              #->rvfi_trap
//                      #              #->rvfi_halt
//                      #              #->rvfi_intr
//                      #              #=>rvfi_rs1_rdata[31:0]
//                      #              #=>rvfi_rs2_rdata[31:0]
//                      #              #=>rvfi_rd_wdata[31:0]
//                      #              #=>rvfi_pc_rdata[31:0]
//                      #              #=>rvfi_pc_wdata[31:0]
//                      #              #=>rvfi_mem_addr[31:0]
//                      #              #=>rvfi_mem_rdata[31:0]
//                      #              #=>rvfi_mem_wdata[31:0]
//fetch_enable_i      ->#              #
//                      #              #->core_sleep_o
//                      ################

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
MIT License or 3-Cause BSD License

## Author
I don't open my real name, so SpyderSika is the name at github.
