#include <Arduino.h>

#include "Tricorder.h"
#include "FartSensor.h"

Tricorder *tricorder;

void setup() {
  Serial.begin(115200);

  tricorder = new Tricorder();
  tricorder->AddSensor(new FartSensor(tricorder, ADC1_CHANNEL_6)); // GPIO 34

  tricorder->Start();
}

void loop() {
  tricorder->Update();

  delay(1000);
}

