#include <Servo.h>

Servo servo1; //MD
Servo servo2; //MI
Servo servo3; //RD
Servo servo4; //RI
Servo servo5; //C

const int ledMD = 13;
const int ledMI = 8;
const int ledRD = 2;
const int ledRI = 4;
const int ledC = 11;

String MDC, MIC, RDC, RIC, CAC;
int angulo;
 
void setup(){
  Serial.begin(115200);
  
  pinMode(ledMD, OUTPUT);
  pinMode(ledMI, OUTPUT);
  pinMode(ledRD, OUTPUT);
  pinMode(ledRI, OUTPUT);
  pinMode(ledC, OUTPUT);
  
  servo1.attach(9);
  servo2.attach(3);
  servo3.attach(10);
  servo4.attach(5);
  servo5.attach(6);
  
  servo1.write(5);
  servo2.write(5);
  servo3.write(15);
  servo4.write(15);
  servo5.write(90);
}
 
void loop(){
  
  digitalWrite(ledMD, LOW);
  digitalWrite(ledMI, LOW);
  digitalWrite(ledRD, LOW);
  digitalWrite(ledRI, LOW);
  digitalWrite(ledC, LOW);
  
  //si existe información pendiente
 if (Serial.available()>0){
    //leemos los datos del serial
    //MDC = Serial.readStringUntil('\n');
    /*MIC = Serial.readStringUntil(' ');
    RDC = Serial.readStringUntil(' ');
    RIC = Serial.readStringUntil(' ');
    CAC = Serial.readStringUntil('\n');
*/
    //Serial.println(MDC);
    digitalWrite(ledMD, HIGH);
    
    //Escribimos en el servo correspondiente
    if(MDC >= "0" && MDC <= "400"){
      angulo = map(MDC.toInt(), 0, 400, 0, 175);
      servo1.write(angulo);
      digitalWrite(ledMD, HIGH);
    }

    if(MIC >= "0" && MIC <= "400"){  
      angulo = map(MIC.toInt(), 0, 400, 5, 175);
      servo2.write(angulo);
      digitalWrite(ledMI, HIGH);
    }

    if(RDC >= "300" && RDC <= "490"){
      angulo = map(RDC.toInt(), 300, 490, 5, 175);
      servo3.write(angulo);
      digitalWrite(ledRD, HIGH);
    }

    if(RIC >= "300" && RIC <= "490"){
      angulo = map(RIC.toInt(), 300, 490, 5, 175);
      servo4.write(angulo);
      digitalWrite(ledRI, HIGH);
    }

    if(CAC >= "250" && CAC <= "350"){
      angulo = map(CAC.toInt(), 250, 350, 5, 175);
      servo5.write(angulo);
      digitalWrite(ledC, HIGH);
    }
  }
}
