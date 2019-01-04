#include <Arduino.h>

#include "Tricorder.h"
#include "FartSensor.h"
#include "Blinker.h"

Tricorder *tricorder;

void setup() {
  Serial.begin(115200);

  tricorder = new Tricorder();

  FartSensor* fartSensor = new FartSensor(tricorder, ADC1_CHANNEL_6, 15);// GPIO 34, 2

  tricorder->AddSensor(fartSensor); 
  tricorder->AddSensor(new Blinker(tricorder));

  tricorder->Start();

  fartSensor->Enable();
}

void loop() {
  tricorder->Update();

  delay(100);
}

