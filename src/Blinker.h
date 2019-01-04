#ifndef __TRICORDER_H__
#include "Tricorder.h"
#endif

#ifndef __BLINKER_H__
#define __BLINKER_H__

#include <Arduino.h>

class Blinker : public TricorderSensor{


private:
    uint8_t _pin, _onValue, _offValue;
    ulong _duration;
    bool _isOn = false;
    ulong _lastChangeTime;

public:
    Blinker(Tricorder *tricorder, uint8_t pin, ulong duration, uint8_t onValue, uint8_t offValue){
        pinMode(pin, OUTPUT);
        _pin = pin;
        _duration = duration;
        _onValue = onValue;
        _offValue = offValue;
        _lastChangeTime = millis();
    }

    Blinker(Tricorder *tricorder, uint8_t pin, ulong duration) : Blinker(tricorder, pin, duration, LOW, HIGH){
    }

    Blinker(Tricorder *tricorder, uint8_t pin) : Blinker(tricorder, pin, 1000, LOW, HIGH) {
    }

    Blinker(Tricorder *tricorder) : Blinker(tricorder, LED_BUILTIN){
    }

    virtual void Update(){
        ulong now = millis();

        if (now - _lastChangeTime >= _duration){
            _isOn = !_isOn;

            digitalWrite(_pin, _isOn ? _onValue : _offValue);

            _lastChangeTime = now;

              Serial.print(_isOn ? "  ON  " : "  off  ");

        }
    }

    bool IsOn(){
        return _isOn;
    }
};
#endif