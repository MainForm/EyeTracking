from SocketClient import *
from SocketFunc import *
import sys

import socket
import cv2

class ControlCamera(SocketClient):
    def __init__(self):
        super().__init__()
        self.camera = cv2.VideoCapture()

    def __del__(self):
        self.camera.release()
        return super().__del__()

    def CameraOpen(self,idx :int):
        return self.camera.open(idx)

    def RecvData(self,client : socket.socket,data : str):
        print(data,' command is received')
        if data == 'frame':
            ret, frame = self.camera.read()

            if ret == True:
                sendImage(client,frame)

try:
    if __name__ == '__main__':
        client = ControlCamera()
        
        client.connect('127.0.0.1',8456)
        client.CameraOpen(int(sys.argv[1]))
        #client.CameraOpen(0)

        while client.isConnceted:
            key = input('command : ')
            if key == 'q':
                break
            
            if key == 'status':
                print(client.isConnceted)
        
finally:
    client.camera.release()
    client.disconnect()