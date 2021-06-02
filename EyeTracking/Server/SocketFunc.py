import socket
import cv2
import numpy as np

#정수를 보내기 위한 함수
def sendInt(socket :socket.socket, data : int):
   	return socket.send((str(data) + '\0').encode()) #숫자 끝에 NULL문자를 보내 숫자의 끝을 나타낸다.


def sendImage(socket :socket.socket,img,quality:int = 90):	
	'''
	1. 사진을 imendcode()함수로 인코딩
	2. 사진의 크기를 먼저 송신
	3. 사진을 송신
	'''
	encode_flag = (cv2.IMWRITE_JPEG_QUALITY,quality)
	ret , encode_data = cv2.imencode('.jpg',img,encode_flag)

	#failed to encode img at imencode() func.
	if ret == False:
		return False
	
	bImgData = encode_data.tostring()
	
	try:
		sendInt(socket, len(bImgData))

		socket.send(bImgData)
	except ConnectionResetError:	#사진 전송 실패시
		return False

	return True		#사진 전송 성공
    
def recvInt(sock : socket.socket) -> int:
	return int(recvString(sock))

def sendString(sock : socket.socket,str : str):
	return sock.send((str + '\0').encode())

def recvString(sock :socket.socket):
	bData = bytearray()

	while True:
		aByte = sock.recv(1)
		if aByte is None:
			return None
		if aByte[0] == 0:
			break
		bData.append(aByte[0])


	return bData.decode()

def recvImg(sock : socket.socket):
	ImgSize = recvInt(sock)
	bData = bytearray()
	
	while len(bData) < ImgSize:
		bData += bytearray(sock.recv(ImgSize - len(bData)))

	bData = bytes(bData)	#convert bytearry to bytes

	Img = np.frombuffer(bData,dtype=np.dtype(np.uint8))
	Img = Img.reshape(ImgSize,1)
	return cv2.imdecode(Img,cv2.IMREAD_COLOR)