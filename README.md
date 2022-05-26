# Framework
 **A C# framework** that communicate with the underlying hardware
 ________________________________________________________________
 
 The **basic logic** and structure of the framework is:
 - At the lowest level the Hardware resource is responsible of the communication (e.g. modbus, OPC UA, tcp, ...)
 - In combination with the resource, various type of channels are present (digital and analog input and output channel specialized in the associated resource)
 - Above this stage, there are general input/output digital and analog channel which may be connected to the specified ones and can be used in various part of the application
 - In parallel to this, there is the ServiceBroker, that can store various property to make them available in each part of the application

This represent the core logic that should be followed and used in higher level of each application.
 
 _________________________________________________________________

 A logger and, at an higher level, UI componenents and a basic common UI interface are available. <br/>
 In addition to this, the framework offers other functionalities such as a database connection (tested only with Microsoft SQL Management Studio, at this stage). <br/>
 Also, some extension methods are provided (like system methods, collection methods, async/await and parallel loops, ecc...) and some task implementation (like a more precise versione of Task.Delay)

__________________________________________________________________

The **tested** (with real hardware) and working classes are:
- Bag
- ServiceBroker (these two are contained in Core.DataStructures)
- Channels and converters
- Modbus resource
- PCanResource
- Logger and (basic) IO

Other tested (without any hardware) classes are:
- Mathematics
- Parameters
- Signal.Processing
- Some functionalities in Core.Threading

__________________________________________________________________
**Configuration file**: <br/>
The configuration handling parse a json file and get the stored configuration. <br/>
Here's an eaxmple of a configuration file:
```json
{
    "Items":
    [
        {
            "Name": "Z32",
            "SharedFolder": "C:\\Users\\simod\\Desktop\\Old\\CopyAndPasteTest",
            "IpAddress": "localhost"
        },
        {
            "Name": "Rest",
            "Port": 8080
        }
    ]
}
```
Each element must have a non empty Name value that will be the configuration item code. <br/>
In C# each element can be accessed by using the json field name. For example:
```cs
string ipAddress = config.Items["Z32"].Value.IpAddress;
int port = config.Items["Rest"].Value.Port;
```