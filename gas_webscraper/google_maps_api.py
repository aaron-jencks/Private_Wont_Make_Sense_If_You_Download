from WebsiteDescriptor import *

import googlemaps
from googlemaps import places
from tqdm import tqdm
import time


gmaps = googlemaps.Client(key='yourKeyHere')


def find_station(browser, place_id: str) -> Website:
    """Finds a single station given a place_id and returns a website"""

    info = places.place(gmaps, place_id)['result']

    name = info['name']
    address = info['formatted_address']
    brand = info['name']  # TODO Create a map of station names to brands
    location = info['geometry']['location']
    rating = info['rating'] if 'rating' in info else 0
    rating_count = info['user_ratings_total'] if 'user_ratings_total' in info else 0
    url = info['url']

    browser.get(url)
    time.sleep(2)
    for gas_type in browser.find_elements_by_css_selector('div.section-gas-prices-price'):
        if gas_type.find_element_by_class_name('section-gas-prices-label').text == 'Regular':
            price = gas_type.find_element_by_tag_name('span').text
            if len(price) > 1:
                price = float(price[1:])
            else:
                price = NaN
            return Website(name, brand,
                           address, location['lat'], location['lng'],
                           price,
                           rating, rating_count)


def find_station_ids() -> list:
    """Finds a list of all the place ids of the stations around town"""

    print("Collecting gas station data.")
    stations = []

    print('Collecting result data')
    time.sleep(2)
    search_result = places.places_nearby(gmaps, '41.977576,-91.666851', 160935, keyword='gas station')
    iter = 1
    while True:
        stations += search_result['results']
        if 'next_page_token' not in search_result:
            break
        else:
            iter += 1
            print("Collecting page {}".format(iter), end='\r')
            token = search_result['next_page_token']
            time.sleep(1)
            while True:
                try:
                    search_result = places.places_nearby(gmaps, '41.977576,-91.666851', 160935, keyword='gas station',
                                                         page_token=token)
                    break
                except googlemaps.exceptions.ApiError as e:
                    continue

    result = []
    print("Filtering bad data")
    for s in tqdm(stations):
        info = places.place(gmaps, s['place_id'])['result']

        if info is not None and info['name'] != '':
            result.append(s['place_id'])
        else:
            print("Found a dud station entry, skipping")

    return result


def find_stations(browser) -> list:
    """Finds all of the gas stations in the state of Iowa and returns them as a list"""

    stations = find_station_ids()

    print('\nScraping statistics:')
    return [find_station(browser, s) for s in tqdm(stations)]


if __name__ == '__main__':
    from selenium import webdriver

    browser = webdriver.Firefox()
    stations = find_stations(browser)
    for s in stations:
        print(str(s))

    browser.close()
