import cv2

import math

def GetCenterOfRetina(image):
    image = cv2.cvtColor(image,cv2.COLOR_BGR2GRAY)

    #전처리
    src = cv2.GaussianBlur(image,(5,5),9)

    threslevel = 1
    kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE,(7,7),anchor=(-1,-1))

    #알고리즘
    try:
        img = src.copy()
        _ , thres = cv2.threshold(img,threslevel,255,cv2.THRESH_BINARY)
        morp = cv2.morphologyEx(thres,cv2.MORPH_OPEN,kernel,iterations=5)
        dst = cv2.bitwise_not(morp)
        _,contours,hierarchy = cv2.findContours(dst,cv2.RETR_TREE,cv2.CHAIN_APPROX_NONE)
        len_contours = len(contours[0])
        sumX = 0
        sumY = 0
        for pt in contours[0]:
            sumX += int(pt[0][0])
            sumY += int(pt[0][1])
    except:
        return None

    cv2.circle(img,(int(sumX / len_contours),int(sumY / len_contours)),20,(255,0,0),1)

    return img
