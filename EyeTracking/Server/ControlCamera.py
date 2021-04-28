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

    def OpenCamera(self,idx : int):
        return self.camera.open(idx)
    
    def ReleaseCamera(self):
        return self.camera.release()

    #데이터를 입력 받을 때
    def RecvData(self,client,data):
        if data == 'frame':
            ret, frame = self.camera.read()
            #dst = self.Algorithm(frame)
            sendImage(client[0],frame)

    def Algorithm(self,frame):
        return frame

try:
    if __name__ == '__main__':
        server = ControlCamera(("",8456))
        
        server.OpenCamera(0)
        server.BeginAccepting()

        print('camera status : ',server.camera.isOpened())

        while True:
            key = input('command : ')

            if key == 'q':
                break
                
            if key == '1':
                server.ShowAllClients()
            elif key == '2':
                print('camera status : ',server.camera.isOpened())
finally:
    server.__del__()