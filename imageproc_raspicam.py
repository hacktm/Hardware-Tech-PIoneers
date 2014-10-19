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
server_address=('localhost',5080)
sock.connect(server_address)

#Initialise camera
with picamera.PiCamera() as camera:
    
    camera.brightness=60
    camera.resolution=(width,height)
    camera.framerate=80
    camera.iso=10
    camera.shutter_speed=camera.exposure_speed
    camera.exposure_mode='off'
    

    while(1):
        stream=io.BytesIO()
        #   print('test1')
        camera.capture(stream,format='jpeg',use_video_port=True)
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
        
        print st
    
        
         
        #cv2.imshow('image',threshot)
        #cv2.waitKey(0)
        #cv2.destroyAllWindows()       
        #sock.close()

        #print('test2')

