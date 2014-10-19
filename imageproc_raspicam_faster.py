import time
import io
import picamera
import picamera.array
import numpy as np
import cv2
import socket
import sys

width=640 
height=480
w_led=32
h_led=32

sock=socket.socket(socket.AF_INET,socket.SOCK_STREAM)
server_address=('10.0.186.75',8000)
sock.connect(server_address)

def outputs():
    while True:
        stream=io.BytesIO()
        yield stream
        data=np.fromstring(stream.getvalue(),dtype=np.uint8)
        img=cv2.imdecode(data,0)

        blur=cv2.GaussianBlur(img,(5,5),0)

        retotsu,threshot=cv2.threshold(blur,0,255,cv2.THRESH_BINARY+cv2.THRESH_OTSU)
        resized=cv2.resize(threshot,(w_led,h_led))
        
        st="img"
        for i in range(0,w_led):
            for j in range(0,h_led):
                px=resized[i,j]
                if px<128:
                    st+='1'
                if px>127:
                    st+='0'
                
        sock.sendall(st)
        print('Sent')
        stream.truncate()
        

with picamera.PiCamera() as camera:
    camera.resolution=(width,height)
    camera.framerate=80
    camera.capture_sequence(outputs(),'jpeg',use_video_port=True)
