from IRCamera import *

import sys

try:
    if __name__ == '__main__':
        server = IRCamera(("",8458))
        server.OpenCamera(2)
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