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
    uint8_t _enablePin;
    BLECharacteristic *_bleSensorCharacteristic, *_bleEnabledCharacteristic;
    bool _isEnabled;

public:
    FartSensor(Tricorder *tricorder, adc1_channel_t channel, uint8_t enablePin){

        adc1_config_width(ADC_WIDTH_BIT_10);   //Range 0-1023 
        adc1_config_channel_atten(channel, ADC_ATTEN_DB_11);  //ADC_ATTEN_DB_11 = 0-3,6V

        pinMode(enablePin, OUTPUT);

        _channel = channel;
        _enablePin = enablePin;

        uint32_t characteristicProperties = BLECharacteristic::PROPERTY_READ |
                      BLECharacteristic::PROPERTY_NOTIFY;

        _bleSensorCharacteristic = tricorder->GetBluetoothService()->createCharacteristic(FARTSENSOR_CHARACTERISTIC_UUID, characteristicProperties);
        _bleEnabledCharacteristic = tricorder->GetBluetoothService()->createCharacteristic(FARTSENSOR_CHARACTERISTIC_UUID, characteristicProperties);
    }

    virtual void Update(){

        int isEnabledInt = _isEnabled ? 1 : 0;

        _bleEnabledCharacteristic->setValue(isEnabledInt);
        _bleEnabledCharacteristic->notify();

        if (_isEnabled){

            int fart = adc1_get_raw(_channel); //Read analog

            Serial.print("*** Fart Value: ");    
            Serial.print(fart);
            Serial.println(" ***");

            _bleSensorCharacteristic->setValue(fart);
            _bleSensorCharacteristic->notify();
        }
    }

    void Enable(){
        digitalWrite(_enablePin, HIGH);

        _isEnabled = true;
    }

    void Disable(){
        digitalWrite(_enablePin, LOW);

        _isEnabled = false;
    }

    bool IsEnabled(){
        return _isEnabled;
    }
};
#endif