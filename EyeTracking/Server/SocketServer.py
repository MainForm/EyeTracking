import socket
import threading
import abc

from SocketFunc import *

class SocketServer(metaclass=abc.ABCMeta):
    #모든 자식클래스에서 공유하는 세마포어
    sem_Clients = threading.Semaphore(1)  

    #생성자
    def __init__(self,addr):
        self.addr = addr    #현재 주소(IP port)
        self.server_socket = socket.socket(socket.AF_INET,socket.SOCK_STREAM)

        self.server_socket.setsockopt(socket.SOL_SOCKET,socket.SO_REUSEADDR,1)
        self.server_socket.bind(addr)

        self.server_socket.listen()

        '''
        key : addr
        value : (socket,addr)
        '''
        self.clients = dict()       #현재 접속중인 클라이언트

    #소멸자
    def __del__(self):
        self.Close()
    
    def Close(self):
        for addr in self.clients.keys():
            self.clients[addr].close()

        self.server_socket.close()

    def BeginAccepting(self):
        #클라이언트 접속 시작
        self.thread_accept = threading.Thread(target=self.AcceptingClient)
        self.thread_accept.daemon = True
        self.thread_accept.start()

    def AcceptingClient(self):
        '''
        클라이언트 접속 쓰레드
        호출 용도 X
        '''
        while True:
            self.sem_Clients.acquire()  #세마포어 진입
            client = self.server_socket.accept()

            self.AcceptedClient(client) #이벤트 발생

            self.clients[client[1]] = client[0] #딕셔너리에 등록
            #접속한 클라이언트만을 위한 쓰레드
            thread_recv = threading.Thread(target=self.RecvFromClient,args=(client[1],))
            thread_recv.daemon = True
            thread_recv.start()

            self.sem_Clients.release()  #세마포어 출구

    def DisconnectClient(self,addr):
        #addr에 해당하는 클라이언트 접속 종료
        #addr = (ip,port)
        if addr in self.clients.keys():#만약 addr로 접속한 client가 있을 때에
            self.DisconnectedClient((self.clients[addr],addr))
            self.sem_Clients.acquire()
            del self.clients[addr]
            self.sem_Clients.release()

    def ShowAllClients(self):
        for key in self.clients:
            print(key)


    def RecvFromClient(self,addr):
        #각 클라이언트에서 데이터 수신 쓰레드
        bBreak = False

        while True:
            recvData = str("")
            try:
                #명령어 수신
                recvData = recvString(self.clients[addr])
            except Exception: #접속 에러
                self.DisconnectClient(addr)
                bBreak = True
            finally:
                if bBreak != True and recvData is None or len(recvData) == 0:#클라이언트 연결 헤제
                    self.DisconnectClient(addr)
                    bBreak = True

            #접속 에러시 명령어 실행 X
            if bBreak == True:
                break                    

            self.sem_Clients.acquire()  #세마포어 진입

            self.RecvData((self.clients[addr],addr),recvData)

            self.sem_Clients.release()  #세마포어 출구

    #이벤트

    #데이터를 입력 받을 때
    def RecvData(self,client,data):
        pass

    #Client가 접속했을 때
    def AcceptedClient(self,client):
        pass

    #Client가 접속을 끊겼을 때
    def DisconnectedClient(self,client):
        pass