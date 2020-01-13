from multiprocessing import Queue, Pool
from queue import Empty, Full
from tqdm import tqdm
import time

from Timer import Timer
from DataController import DataController
from WebsiteController import WebsiteController
from google_search_parser import generate_parsers


if __name__ == "__main__":
    print("Generating parsers")
    parsers = generate_parsers()

    print("Setting up controllers")
    tm = Timer(interval=24*3600)
    dc = DataController(tm.Stop)
    wc = WebsiteController(parsers, tm.Stop)

    tm.start()
    dc.start()
    wc.start()
    print("Waiting for the controllers")
    datum_count = 0
    while True:
        if not tm.Tx.empty():
            try:
                tm.Tx.get_nowait()
                try:
                    wc.Trigger.put_nowait(True)
                except Full as _:
                    print("Website controller is full, skipping...")
            except Empty as _:
                print("Timer magically became empty while retrieving it, skipping")

        while not wc.Tx.empty():
            try:
                datum = wc.Tx.get_nowait()
                try:
                    dc.dataq.put_nowait(datum)
                    datum_count += 1
                    if datum_count >= 60:
                        dc.save()
                        print("Saved data")
                        datum_count = 0
                    # dc.save()
                    # print("Saved data")
                except Full as _:
                    print("Data Controller is full, skipping")
            except Empty as _:
                print("Website controller magically became empty while retrieving it, skipping")

        time.sleep(5)
