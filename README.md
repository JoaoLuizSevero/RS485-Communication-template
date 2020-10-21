# RS485_template

##About##

This is a simple project to exemplify a RS485 communication with MAX485. We send commands from a interface to two nodes, who identify what node is the targer and execute the respective command.
 
##Interface##

The interface to control the nodes was made in **Visual Studio** and send commands via serial to a **USB2.0/RS485** converter.
 
![Interface](https://github.com/JoaoLuizSevero/RS485_template/blob/main/assets/Interface.PNG)

##Assembly##
We use two Arduino NANO as nodes, each one with a MAX485 module and a relay wich will execute the command received from interface.

![Assembly](https://github.com/JoaoLuizSevero/RS485_template/blob/main/assets/node.png)

##Nodes##
The nodes keep listening until the message comes via a software serial, and when it comes, he verify if the message is for him. In positive case, he verify what command is and execute. Note that if its a ON/OFF command he just execute, but if its a request he change to transmit mode for send the message.

'''
#include <SoftwareSerial.h>

#define relay     7
#define TC        2
#define node      '2'

String payload = "";

SoftwareSerial SWSerial(5, 4);

void setup()
{
  SWSerial.begin(115200);

  pinMode(TC, OUTPUT);
  pinMode(relay, OUTPUT);
  
  digitalWrite(TC, LOW);
  digitalWrite(relay, HIGH); 
}

void loop()
{
  if (SWSerial.available())
  {
    while (SWSerial.available()>0)
    {
      payload += (char)SWSerial.read();
    }
  }
  
  if(payload != ""){
    if(payload[0] == node){
      switch(payload[1]){
        case '0':
          digitalWrite(relay, LOW);
          break;
        case '1':
          digitalWrite(relay, HIGH);
          break;
        case '2':
          digitalWrite(TC, HIGH);
          digitalRead(relay)==HIGH? SWSerial.print("RECEIVE: node "+(String)node+" is OFF"):SWSerial.print("RECEIVE: node "+(String)node+" is ON");
          digitalWrite(TC, LOW);
          break;
        default:
          digitalWrite(TC, HIGH);
          SWSerial.print("RECEIVED: command not recognized");
          digitalWrite(TC, LOW);
          break;        
      }
    }
    payload = "";
  }
  
  delay(100);
}
'''
