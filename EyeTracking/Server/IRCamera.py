from ControlCamera import *
from EyeTracking import *

class IRCamera(ControlCamera):
    def Algorithm(self, frame):
        return GetCenterOfRetina(frame)