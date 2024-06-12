from pathlib import Path
import sys

directory_in_str = "D:\\Unity Project\\Goods Sort\\"
# directory_in_str = sys.argv[1]

pathlist = Path(directory_in_str).glob('**/*.*')
for path in pathlist:
     # because path is object not string
    try:
        path_in_str = str(path)
        f = open(path_in_str, "r")
        contents = f.read()
        if contents.find("11.8.0") != -1 or contents.find("11_8_0") != -1:
            print(path_in_str)
        f.close()
    except:
        continue