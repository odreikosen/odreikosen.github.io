import smtplib
import time
import socket
import imapclient
import pyzmail
import os
import webbrowser
import shelve
import pprint
# at&t:     number@mms.att.net
# t-mobile: number@tmomail.net
# verizon:  number@vtext.com
# sprint:   number@page.nextel.com
user=0
messages=list()
part1=list()
hey=list()
convos=list()
imapObj=imapclient.IMAPClient('imap.gmail.com',ssl=True)
server=smtplib.SMTP(socket.gethostbyname('smtp.gmail.com'),587)
server.starttls()
email=input('What is your gmail? ')
email=email+'@gmail.com'
password=input('What is your password? ')
server.login(email,password)
imapObj.login(email,password)
print('Succesful Login')
print('----------------------------------')
while(True):
    number=input('Who are you trying to contact? (type /q to quit) ')
    addressbook=shelve.open('address')
    if number in addressbook.keys():
        name=number
        number=addressbook.get(number)
        convo=open(number+'.txt','a+')
        addressbook.close()
    elif number=='/q':
        break
    else:
        new=input('There is no record of them in your addressbook, is this a new number? (y/n) ')
        if new =='y':
            while(True):        
                carrier=input('What is their carrier? ')
                if carrier =='att':
                    number=number+'@mms.att.net'
                    break
                elif carrier == 'tmobile':
                    number=number+'tmomail.net'
                    break
                elif carrier =='verizon':
                    number=nummber+'vtext.com'
                    break
                elif carrier =='sprint':
                    number=number+'page.nextel.com'
                    break
                else:
                    print('Please choose (att,tmobile,verizon,sprint)')
                    continue
            while (True):
                add=input('Would you like to add them to your address book? (y/n) ')
                if add=='y':
                    name=input("What is their name? ")
                    addressbook[name]= number
                    convo=open(number+'.txt','a+')
                    addressbook.close()
                    break
                elif add=='n':
                    convo=open(number+'.txt','a+')
                    name=number
                    addressbook.close()
                    break
                else:
                    continue
        else:
            continue
    print(' ')
    webbrowser.open(number+'.txt')
    print('Type Your Message Below (type /q to quit), (type /c to check messages)')
    while(True):
        message=input()
        if message== '/q':
            convo.close
            break
        if message=='/c':
            imapObj.select_folder('INBOX',readonly=False)
            replies=imapObj.search(['UNSEEN','FROM',number])
            raw=imapObj.fetch(replies,['BODY[]'])
            for i in range (0,len(replies)):
                messages=messages+[pyzmail.PyzMessage.factory(raw[replies[i]][b'BODY[]'])]
            for i in range (0,len(raw)):
               convos=convos+[messages[i].html_part.get_payload().decode(messages[i].html_part.charset)]
               hey=convos[i][353:len(convos[i])]
               hello=convos[i][352]
               for c in hey:
                   if c=='\n':
                       break
                   hello=hello+c
               part1=part1+[hello]
            part2=open('temp.txt','w+')
            if len(replies) != 0:
                part2.write('\n')
            for i in range (1,len(part1)+1):
                 part2.write(name+': '+ part1[-i])
            part3=open(number+'.txt','r')
            content=part3.read()
            part2.write(content)
            part3.close()
            part2.close()
            part2=open('temp.txt','r')
            content=part2.read()
            convo.close()
            convo=open(number+'.txt','w+')
            convo.write(content)
            part2.close()
            part1=list()
            convos=list()
            messages=list()
            convo.close()
            os.startfile(number+'.txt')
            input('Hit Enter to return to texting')
            os.system('TASKKILL /F /IM notepad.exe')
            convo=open(number+'.txt','a+')
            continue
        server.sendmail('hello',number,message)
        print('Sending...',end=' ')
        for i in range (0,3):
            print('...', end=' ')
            time.sleep(1)
        print('Sent!')
        part2=open('temp.txt','w+')
        part3=open(number+'.txt','r')
        content=part3.read()
        part2.write('Me: '+message+'\n')
        part2.write(content)
        part2.close()
        part3.close()
        part3=open('temp.txt','r')
        content=part3.read()
        part2=open(number+'.txt','w+')
        part2.write(content)
        part2.close()
        part3.close()
                
                


