from ControlCamera import *
from EyeTracking import *

class RGBCamera(ControlCamera):
    def __init__(self, addr) -> None:
        super().__init__(addr)

    def Algorithm(self, frame):
        cv2.waitKey(12)
        return frame