from SocketServer import *

class ControlCamera(SocketServer):
    def __init__(self, addr):
        super().__init__(addr)
        