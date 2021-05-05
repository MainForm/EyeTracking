from ControlCamera import *
from EyeTracking import *

class IRCamera(ControlCamera):
    def __init__(self, addr) -> None:
        super().__init__(addr)

    def Algorithm(self, frame):
        return GetCenterOfRetina(frame)