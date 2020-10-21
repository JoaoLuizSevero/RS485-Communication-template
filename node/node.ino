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
