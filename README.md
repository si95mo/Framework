# Framework
 A C# framework
 
 The basic logic of the framework is:
 - At the lowest level the Hardware resource is responsible of the communication (e.g. modbus, OPC UA, tcp, ...)
 - In combination with the resource, various type of channels are present (digital and analog input and output channel specialized in the associated resource)
 - Above this stage, there are general input/output digital and analog channel which may be connected to the specified ones and can be used in various part of the application
 - In parallel to this, there is the ServiceBroker, that can store various property to make them available in each part of the application
 
 A logger and, at an higher level, UI compoenents and a basic common UI interface are available
