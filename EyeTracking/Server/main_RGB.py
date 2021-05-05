from RGBCamera import *

import sys

try:
    if __name__ == '__main__':
        server = RGBCamera(("",8456))
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