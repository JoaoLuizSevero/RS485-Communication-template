# RS485_template

##About
This is a simple project to exemplify a RS485 communication with MAX485. We send commands from a interface to two nodes, who identify what node is the targer and execute the respective command.
 
##Interface
The interface to control the nodes was made in **Visual Studio** and send commands via serial to a **USB2.0/RS485** converter.
 
![Interface]()

##Assembly
We use two Arduino NANO as nodes, each one with a MAX485 module and a relay wich will execute the command received from interface.

![Assembly]()

##Nodes
The nodes keep listening until the message comes via a software serial, and when it comes, he verify if the message is for him. In positive case, he verify what command is and execute. Note that if its a ON/OFF command he just execute, but if its a request he change to transmit mode for send the message.
