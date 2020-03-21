from WebsiteParser import WebsiteParser
from WebsiteDescriptor import *
from simple_web import simple_get
from defs import crude_price_url
from google_maps_api import find_station_ids, find_station

from selenium import webdriver
from selenium.webdriver.firefox.options import Options

import os
import datetime as dt


class GoogleParser(WebsiteParser):
    """Represents a basic parser to collect information for the scraper by parsing a google search result for nearby
    gas stations and finds their affiliation, address and price.  It also finds the crude oil barrel price"""

    browser = None
    prev_oil_price = 0
    prev_oil_check_day = -1

    def __init__(self, place_id: str):
        super().__init__()
        self.place = place_id
        if GoogleParser.browser is None:
            foptions = Options()
            foptions.headless = True
            GoogleParser.browser = webdriver.Firefox(options=foptions)

    def __del__(self):
        if self.browser:
            GoogleParser.browser.close()
            GoogleParser.browser = None

    @staticmethod
    def retrieve_crude_price() -> OilWebsite:
        doy = dt.datetime.now().timetuple().tm_yday
        if doy != GoogleParser.prev_oil_check_day:
            GoogleParser.prev_oil_check_day = doy
            parser = simple_get(crude_price_url)
            try:
                GoogleParser.prev_oil_price = float(parser.find('span', class_='push-data').text)
            except Exception as e:
                print(e)
                print("Collecting crude oil price failed...")
                GoogleParser.prev_oil_price = NaN
                return OilWebsite()

        return OilWebsite(GoogleParser.prev_oil_price)

    def parse(self) -> list:

        print("Looking up: {}".format(self.place))

        oil = self.retrieve_crude_price()

        ws = find_station(self.browser, self.place)
        return [WebsiteDescriptor(ws, oil)] if ws is not None and oil is not None else []


def import_parsers(filename: str = './places.json') -> list:
    """Imports all of the place_ids of the given json file and converts them to GoogleParsers"""
    result = []
    with open(filename, mode='r') as fp:
        for l in fp.read().splitlines(keepends=False):
            result.append(GoogleParser(l))
    return result


def export_parsers(parsers: list, filename: str = './places.json'):
    """Exports all of the given parsers to the given json file"""
    with open(filename, mode='w+') as fp:
        fp.writelines([p.place + '\n' for p in parsers])


def generate_parsers(filename: str = './places.json') -> list:
    """Creates a series of GoogleParsers based on the results of a google maps api search"""

    if os.path.exists(filename):
        print("Reading in premade place_ids from {}".format(filename))
        return import_parsers(filename)
    else:
        print("Generating new place_ids")
        ws = find_station_ids()
        parsers = [GoogleParser(i) for i in ws]
        print("Saving places to file {}".format(filename))
        export_parsers(parsers, filename)
        return parsers



