# Starter file for ex2.asm

.data 
input1: .space 22
input2: .space 22
newline: .asciiz "\n"	

        
.text 0x3000

main:
	#----------------------------------------------------------------#
	# Write code here to do exactly what main does in the C program.
	#
	# Please follow these guidelines:
	#
	#	Use syscall 8 to do the job of fgets()
	#	Then call h_to_i to perform conversion from hex string to integer
	#	Then call NchooseK to compute its factorial
	#	Then use syscall 1 to print the factorial result
	#----------------------------------------------------------------#
	ori $sp,$0,0x3000
	addi $fp,$sp,0
	addi $v0,$0,8	
	li $a1,22
	la $a0,input1
	syscall			#reads input1
	
	la $a0,input1
	jal h_to_i
	ori $s0,$v0,0
	beq $s0,$0,end
	
	addi $v0,$0,8	
	li $a1,22
	la $a0,input2
	syscall			#reads input2
	
	
	
	la $a0,input2
	jal h_to_i
	ori $s1,$v0,0
	
	ori $a0,$s0,0
	ori $a1,$s1,0
	addi $t1,$0,0
	addi $t2,$0,0
	jal NchooseK
	
	ori $t1,$v0,0
	addi $a0,$t1,0
	addi  $v0,$0,1
	syscall
	
	addi $2,$0,4
	la $a0,newline
	syscall
	
	j main
	

end: 
	ori   $v0, $0, 10     # system call 10 for exit
	syscall               # we are out of here.



h_to_i:
	#----------------------------------------------------------------#
	# $a0 has address of the string to be parsed.
	#
	# Write code here to implement the function you wrote in C.
	# Since this is a leaf procedure, you may be able to get away
	# without using the stack at all.
	#
	# $v0 should have the integer result to be returned to main.
	#----------------------------------------------------------------#
	ori $t1,$a0,0
	addi $t3,$0,60			#t3 is 60, ascii divider of number and character
	addi $t4,$0,0			#t4 is sum intialized as 0
	addi $t9,$0,10			#t9 is 10 used for comparison
	

loop:
	lb $t7,0($t1)
	ori $t2,$t7,0			#number in t2
	beq $t2,$t9,return
	sle $t6,$t2,$t3
	beq $t6,$0,character
	addi $t2,$t2,-48
alg:
	sll $t4,$t4,4
	or $t4,$t2,$t4
	addi $t1,$t1,1
	j loop
return:
	ori $v0,$t4,0
	jr	$ra

character:
	addi $t2,$t2,-87
	j alg



NchooseK:
	#----------------------------------------------------------------#
	# $a0 has the number n, $a1 has k, from which to compute n choose k
	#
	# Write code here to implement the function you wrote in C.
	# Your implementation need NOT be recursive; a simple iterative
	# implementation is sufficient.  Hence, this may be a leaf
	# procedure, and you may be able to get away without using the
	# stack at all.
	#
	# $v0 should have the NchooseK result to be returned to main.
	#----------------------------------------------------------------#
	beq $a0,$a1, returner
	beq $a1,0,returner
	addi $sp,$sp,-8
	sw $ra,4($sp)
	sw $fp,0($sp)
	addi $fp,$sp,4
	addi $sp,$sp,-8
	sw $a1,4($sp)
	sw $a0,0($sp)
	addi $a0,$a0,-1
	addi $a1,$a1,-1
	jal NchooseK
	ori $t1,$v0,0
	lw $a0,0($sp)
	lw $a1,4($sp)
	addi $a0,$a0,-1
	addi $sp,$sp,-4
	sw $t1,0($sp)
	
	jal NchooseK
	ori $t2,$v0,0
	
	
	lw $t1,0($sp)
	add $v0,$t1,$t2
	addi $sp,$fp,4
	lw $t1,-12($fp)
	lw $a0,-8($fp)
	lw $a1,-4($fp)
	lw $ra,0($fp)
	lw $fp,-4($fp)
	
	jr $ra
	
	
returner:
	addi $v0,$0,1
	jr $ra