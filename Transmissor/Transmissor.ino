#include <SoftwareSerial.h>

#define SRX       4
#define STX       5
#define TC        2
#define Transmit  HIGH
#define Receive   LOW

SoftwareSerial SWSerial(SRX, STX);

void setup()
{
  SWSerial.begin(4800);

  pinMode(TC, OUTPUT);
  pinMode(A0, INPUT_PULLUP);
}

void loop()
{
  int  valor = digitalRead(A0);
  if (valor == 0)
  {
    digitalWrite(TC, Transmit);
    SWSerial.println("Botao acionado");
    digitalWrite(TC, Receive);
    
    while (digitalRead(A0) == 0)
    {
      delay(50);
    }
  }
}
