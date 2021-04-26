import socket
import threading
import abc

from SocketFunc import *

class SocketClient(metaclass=abc.ABCMeta):

    #생성자
    def __init__(self):
        self.client_socket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
        self.isConnceted = False

    #소멸자
    def __del__(self):
        self.client_socket.close()
    
    def connect(self,IP, port):

        try:
            self.IP = IP
            self.port = port

            self.client_socket.connect((IP,port))
            self.isConnceted = True

            self.td_receiving = threading.Thread(target=self.receivingData)
            self.td_receiving.daemon =True
            self.td_receiving.start()

            self.ConnectedServer(self.client_socket)
        except socket.error as e:
            print('socket.error is occured in connect() : ',e)
            self.disconnect()
        except Exception as e:
            print('Exception is occured in connect() : ',e)
            self.disconnect()

    def disconnect(self):
        self.isConnceted = False
        self.DisconnectedServer(self.client_socket)
        self.client_socket.close()

    def receivingData(self):
        try:
            while self.isConnceted:
                cmd = recvString(self.client_socket)

                if cmd is None or len(cmd) == 0:
                    self.disconnect()
                    break

                self.RecvData(self.client_socket,cmd)

        except socket.error as e:
            print('socket.error is occured in receivingData() : ',e)
            self.disconnect()
        except Exception as e:
            print('Exception is occured in receivingData() : ',e)
            self.disconnect()

            
    #이벤트

    #데이터를 입력 받을 때
    def RecvData(self,client : socket.socket,data : str):
        pass

    #Client가 접속했을 때
    def ConnectedServer(self,client: socket.socket):
        pass

    #Client가 접속을 끊겼을 때
    def DisconnectedServer(self,client: socket.socket):
        pass