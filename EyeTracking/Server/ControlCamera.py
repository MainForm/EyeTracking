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
        self.idx = idx
        return self.camera.open(idx)
    
    def ReleaseCamera(self):
        return self.camera.release()

    #데이터를 입력 받을 때
    def RecvData(self,client,data):
        if data == 'frame':
            # try:
            #     max = self.camera.get(cv2.CAP_PROP_FRAME_COUNT)
            #     nPos = self.camera.get(cv2.CAP_PROP_POS_FRAMES)
            #     if max == nPos:
            #         self.camera.open(self.idx)
            # except:
            #     pass
            ret, frame = self.camera.read()

            dst = self.Algorithm(frame)
            sendImage(client[0],dst)

    def Algorithm(self,frame):
        return frame