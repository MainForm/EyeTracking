from SocketServer import SocketServer
from SocketFunc import *

import socket
import cv2

class ControlCamera(SocketServer):
    def __init__(self,addr) -> None:
        super().__init__(addr)
        self.camera = cv2.VideoCapture()

    def __del__(self):
        self.camera.release()
        return super().__del__()

    def CameraOpen(self,idx : int):
        return self.camera.open(idx)

    #데이터를 입력 받을 때
    def RecvData(self,client,data):
        if data == 'frame':
            ret, frame = self.camera.read()
            dst = self.Algorithm(frame)
            sendImage(client[0],dst)

    def Algorithm(self,frame):
        return frame

try:
    if __name__ == '__main__':
        server = ControlCamera(("",8456))
        
        server.CameraOpen(0)
        server.BeginAccepting()

        while True:
            key = input('command : ')

            if key == 'q':
                break
                
            if key == '1':
                server.ShowAllClients()
finally:
    server.__del__()