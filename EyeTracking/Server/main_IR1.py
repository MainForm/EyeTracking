from IRCamera import *

import sys

try:
    if __name__ == '__main__':
        server = IRCamera(("",8457))
        server.OpenCamera('테스트1.mp4')
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
            elif key == 'r':
                server.SendCommand('Calibration',0,[1])
            elif key == 'p':
                x,y = input('point : ').split()
                server.SendCommand('EyePoint',0,[x,y])
finally:
    server.__del__()