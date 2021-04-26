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
        self.IP = IP
        self.port = port

        self.client_socket.connect((IP,port))

        self.td_receiving = threading.Thread(target=self.receivingData)
        self.td_receiving.daemon =True
        self.td_receiving.start()

        self.ConnectedServer(self.client_socket)
        self.isConnceted = True

    def disconnect(self):
        self.isConnceted = False
        self.DisconnectedServer(self.client_socket)
        self.client_socket.close()

    def receivingData(self):
        while self.isConnceted:
            try:
                cmd = recvString(self.client_socket)

                if len(cmd) == 0:
                    self.disconnect()
                    break

                self.RecvData(self.client_socket,cmd)

            except socket.error as e:
                print('socket.error is occured in receivingData() : ',e)
            except Exception as e:
                print('Exception is occured in receivingData() : ',e)
            
    #이벤트

    #데이터를 입력 받을 때
    def RecvData(self,client,data):
        pass

    #Client가 접속했을 때
    def ConnectedServer(self,client):
        pass

    #Client가 접속을 끊겼을 때
    def DisconnectedServer(self,client):
        pass