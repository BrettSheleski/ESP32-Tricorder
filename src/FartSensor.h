#ifndef __TRICORDER_H__
#include "Tricorder.h"
#endif

#ifndef __FARTSENSOR_H__
#define __FARTSENSOR_H__

#include <Arduino.h>
#include <BLEServer.h>
#include <driver/adc.h>

#define FARTSENSOR_CHARACTERISTIC_UUID "0051bd71-ed61-4b91-86e6-07154a2c441d"

class FartSensor : public TricorderSensor{


private:
    adc1_channel_t _channel;
    BLECharacteristic *_bleCharacteristic;

public:
    FartSensor(Tricorder *tricorder, adc1_channel_t channel){

        adc1_config_width(ADC_WIDTH_BIT_10);   //Range 0-1023 
        adc1_config_channel_atten(channel, ADC_ATTEN_DB_11);  //ADC_ATTEN_DB_11 = 0-3,6V

        _channel = channel;
        uint32_t characteristicProperties = BLECharacteristic::PROPERTY_READ |
                      BLECharacteristic::PROPERTY_NOTIFY;

        _bleCharacteristic = tricorder->GetBluetoothService()->createCharacteristic(FARTSENSOR_CHARACTERISTIC_UUID, characteristicProperties);
    }

    virtual void Update(){
        int fart = adc1_get_raw(_channel); //Read analog

        Serial.print("*** Fart Value: ");    
        Serial.print(fart);
        Serial.println(" ***");

        _bleCharacteristic->setValue(fart);
        _bleCharacteristic->notify();
    }
};
#endif