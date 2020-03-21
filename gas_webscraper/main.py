from multiprocessing import Queue, Pool
from queue import Empty, Full
from tqdm import tqdm
import time

from Timer import Timer
from DataController import DataController
from mod_WebsiteController import WeeklyUpdateWebsiteController
from google_search_parser import generate_parsers


if __name__ == "__main__":
    print("Generating parsers")
    parsers = generate_parsers()

    print("Setting up controllers")
    tm = Timer(interval=24*3600)
    tm2 = Timer(tm.Stop, interval=24*3600)
    dc = DataController(tm.Stop)
    wc = WeeklyUpdateWebsiteController(parsers, tm.Stop, dc.dataq, tm.Tx)

    tm.start()
    tm2.start()
    dc.start()
    wc.start()
    print("Waiting for the controllers")
    datum_count = 0
    while True:
        if not tm2.Tx.empty():
            try:
                tm2.Tx.get_nowait()
                dc.save()
                print("Saved data")
            except Empty as _:
                print("Timer magically became empty while retrieving it, skipping")

        time.sleep(5)
