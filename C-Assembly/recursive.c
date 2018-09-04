#include<stdio.h>
#include<string.h>
char input1[22];     // To read in a string of up to 20 hexits plus newline and null
char input2[22];     // To read in a string of up to 20 hexits plus newline and null

int h_to_i(char* str);
int NchooseK(int n, int k);

int main() {

  int n, k, result;
  int a=1;
  while(a==1){


                    // Do the following inside a loop

    fgets(input1, 22, stdin);



    n = h_to_i(input1);
    if(n==0){
      break;
    }


    fgets(input2, 22, stdin);



    k = h_to_i(input2);


    result = NchooseK(n, k);



    printf("%d\n", result);


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


int NchooseK(int n, int k) {
  if(n==k){
  return 1;
  }
  if(k==0){
  return 1;
  }

  else{
  return (NchooseK(n-1,k-1) + NchooseK(n-1,k));
  }

}
