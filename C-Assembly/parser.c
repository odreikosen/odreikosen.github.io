#include<stdio.h>
#include<string.h>
char input[22];     // To read in a string of up to 20 hexits plus newline and null

int h_to_i(char* str);

int main() {
	int a=1;
  int value;

                    // Do the following inside a loop
while(a==1){
    fgets(input, 22, stdin);


    value = h_to_i(input);
	if(value==0){
break;
}
    printf("%d\n", value);
}
}


int h_to_i(char* str) {
  int sum=0;
  int alg;
  for(int i=0;i<strlen(str)-2;i++){
    if(str[i]<60){
      alg=str[i]-48;
    } else{
alg=str[i]-87;
    }
    sum=sum+alg;
    sum=sum*16;
  }


  if(str[strlen(str)-2]<60){
    alg=str[strlen(str)-2]-48;
  } else{
    alg=str[strlen(str)-2]-87;
 }
  sum=sum+alg;
  return sum;
}
