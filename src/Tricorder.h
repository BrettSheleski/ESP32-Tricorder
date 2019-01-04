#ifndef __TRICORDER_H__
#define __TRICORDER_H__

#include <BLEDevice.h>
#include <BLEServer.h>
#include <Arduino.h>

#define TRICORDER_MAX_SENSOR_COUNT              4 
#define TRICORDER_DEFAULT_NAME "Tricorder"
#define TRICORDER_BLE_SERVICE_UUID              "61879ac8-5b8e-40bb-8165-53b531cf02bf"


class TricorderSensor{
    public:
    virtual void Update() = 0;
};

class TricorderBLEServerCallbacks : public BLEServerCallbacks{
    private:
    bool _deviceConnected;

    public:
    void onConnect(BLEServer* pServer) {
      _deviceConnected = true;
      Serial.println("*** Connected :) ***");
    };

    void onDisconnect(BLEServer* pServer) {
      _deviceConnected = false;
      Serial.println("*** Disconnected :( ***");
    }

    bool IsDeviceConnected(){
        return _deviceConnected;
    }
};


class Tricorder
{
private:
    BLEServer* _bleServer;
    BLEService *_bleService;
    bool _isBluetoothConnected;
    TricorderSensor *_sensors[TRICORDER_MAX_SENSOR_COUNT];
    int _sensorCount;
    TricorderBLEServerCallbacks *_serverCallbacks;


   void Init(BLEServer *server){
        
        _bleServer = server;
        _bleService = _bleServer->createService(TRICORDER_BLE_SERVICE_UUID);
        
        _serverCallbacks = new TricorderBLEServerCallbacks();   

        _bleServer->setCallbacks(_serverCallbacks);
        _sensorCount = 0;
    }    

public:

    Tricorder(BLEServer *server){
        Init(server);
    }

    Tricorder(){

        if (!BLEDevice::getInitialized()){
            BLEDevice::init(TRICORDER_DEFAULT_NAME);
        }

        BLEServer* server = BLEDevice::createServer();

        Init(server);
    }

    Tricorder(char *name){
        if (!BLEDevice::getInitialized()){
            BLEDevice::init(name);
        }
        
        BLEServer* server = BLEDevice::createServer();

        Init(server);
    }

    ~Tricorder(){

    }

    void Start(){
        _bleService->start();
        _bleServer->getAdvertising()->start();
    }

    void Update(){
        for(int i = 0; i < _sensorCount; ++i){
            _sensors[i]->Update();
        }
    }

    bool IsBluetoothConnected(){
        return _serverCallbacks->IsDeviceConnected();
    }
    
    BLEService* GetBluetoothService(){
        return _bleService;
    }

    BLEServer* GetBluetoothServer(){
        return _bleServer;
    }

    void AddSensor(TricorderSensor *sensor){
        _sensors[_sensorCount] = sensor;
        ++_sensorCount;
    }
};

#endif