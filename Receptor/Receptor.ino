#include <SoftwareSerial.h>

#define SRX       4
#define STX       5
#define TC        2
#define Transmit  HIGH
#define Receive   LOW

#define relay  7

SoftwareSerial SWSerial(SRX, STX);

String inputString = "";
boolean stringComplete = false;

void setup()
{
  Serial.begin(115200);
  SWSerial.begin(4800);
  
  pinMode(relay, OUTPUT);
  pinMode(TC, OUTPUT);
  
  digitalWrite(TC, Receive);

  Serial.println("Modulo Receptor");
  Serial.println("Aguardando dados...");
}

void loop()
{
  if (SWSerial.available())
  {
    while (SWSerial.available())
    {
      char inChar = (char)SWSerial.read();
      inputString += inChar;
      
      if (inChar == 'n')
      {
        Serial.print(inputString);
        stringComplete = true;
        inputString = "";
        digitalWrite(relay, HIGH);
        delay(500);
        digitalWrite(relay, LOW);
      }
    }
  }
}
