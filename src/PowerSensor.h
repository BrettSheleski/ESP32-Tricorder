#ifndef __TRICORDER_H__
#include "Tricorder.h"
#endif

#ifndef __POWERSENSOR_H__
#define __POWERSENSOR_H__

#include <Arduino.h>
#include <BLEServer.h>
#include <driver/adc.h>

#define POWERSENSOR_SERVICE_UUID "b7c87332-f76f-4c3e-96a5-201b0b40f356"
#define POWERSENSOR_BATTERY_LEVEL_CHARACTERISTIC_UUID "dd97f21e-7378-434b-a585-37e7717e40cb"
#define POWERSENSOR_CHARGE_STATE_CHARACTERISTIC_UUID "b140c954-1db3-4d7b-b123-9dc643e598bf"
#define POWERSENSOR_BATTERY_LEVEL_MAX 1023.0


class PowerSensor : public TricorderSensor{


private:
    adc1_channel_t _batteryLevelPin;
    uint8_t _chargeIndicatorPin;
    BLECharacteristic *_bleBatteryLevelCharacteristic, *_bleChargeStateCharacteristic;
    BLEService *_bleService;
    

public:
    PowerSensor(Tricorder *tricorder, adc1_channel_t batteryLevelPin, uint8_t chargeIndicatorPin) {
        _batteryLevelPin = batteryLevelPin;
        _chargeIndicatorPin = chargeIndicatorPin;

        _bleService = tricorder->GetBluetoothServer()->createService(POWERSENSOR_SERVICE_UUID);

        uint32_t characteristicProperties = BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_NOTIFY;
        _bleBatteryLevelCharacteristic = _bleService->createCharacteristic(POWERSENSOR_BATTERY_LEVEL_CHARACTERISTIC_UUID, characteristicProperties);
        _bleChargeStateCharacteristic = _bleService->createCharacteristic(POWERSENSOR_CHARGE_STATE_CHARACTERISTIC_UUID, characteristicProperties);
    }

    virtual void Update(){
        UpdateBatteryLevel();
        UpdateChargeState();
    }

    void UpdateBatteryLevel(){
        int batteryLevel = adc1_get_raw(_batteryLevelPin); //Read analog

        float batteryPercent = batteryLevel / POWERSENSOR_BATTERY_LEVEL_MAX;

        Serial.print("*** Battery Level: ");    
        Serial.print(batteryLevel);
        Serial.println(" ***");

        _bleBatteryLevelCharacteristic->setValue(batteryPercent);
        _bleBatteryLevelCharacteristic->notify();
    }

    void UpdateChargeState(){
        int isUsbConnected = digitalRead(_chargeIndicatorPin);
        
        _bleChargeStateCharacteristic->setValue(isUsbConnected);
        _bleChargeStateCharacteristic->notify();
    }
};
#endif