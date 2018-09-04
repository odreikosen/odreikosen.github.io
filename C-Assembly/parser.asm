.data 0x0

newline: .asciiz "\n"
.text 0x3000
ori $v0,$0,5
syscall
ori $s0,$v0,0
mul $s1,$s0,4
addi $s2,$s1,-4
ori $t0,$0,0
ori $t1,$0,0
ori $a0,$0,0
ori $a1,$0,0
ori $sp,$0,0x2ffc
addi   $fp, $sp, -4 
loop:
sle $t1,$s1,$t0
bnez $t1,endloop
ori $v0,$0,5
syscall
sw $v0,0($t0)
addi $t0,$t0,4
j loop
endloop:
jal quick
ori $t0,$0,0
loop1:
sle $t1,$s1,$t0
bnez $t1,done
ori $v0,$0,1
lw $t4,0($t0)
ori $a0,$t4,0
syscall
addi $2,$0,4
	la $a0,newline
	syscall
	
addi $t0,$t0,4
j loop1



done:
   ori     $v0, $0, 10     	# System call code 10 for exit
    syscall     
quick:
 addi    $sp, $sp, -8        # Make room on stack for saving $ra and $fp
    sw      $ra, 4($sp)         # Save $ra
    sw      $fp, 0($sp)         # Save $fp

    addi    $fp, $sp, 4       

sge $t1,$a1,$s2
bnez $t1,exit
slti $t1,$a1,0
bnez $t1,exit

lw $t2,4($a1)
lw $t3,0($a1)
slt $t1,$t2,$t3
beqz $t1,knot
sw $t2,0($a1)
sw $t3,4($a1)
addi $a1,$a1,-4
jal quick
knot:
addi $a0,$a0,4
addi $a1,$a0,0
jal quick
j exit

exit:
 addi    $sp, $fp, 4     # Restore $sp
    lw      $ra, 0($fp)     # Restore $ra
    lw      $fp, -4($fp)    # Restore $fp
    jr	    $ra
