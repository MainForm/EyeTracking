from SocketClient import *
import sys

import socket

class ControlCamera(SocketClient):
    def __init__(self):
        super().__init__()

    def RecvData(self,client : socket.socket,data):
        print(data)

if __name__ == '__main__':
    client = ControlCamera()

    client.connect('127.0.0.1',8456)

    while client.isConnceted:
        key = input('command : ')

        if key == 'q':
            break

    client.disconnect()