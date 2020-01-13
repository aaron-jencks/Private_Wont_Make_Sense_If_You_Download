import datetime as dt

from numpy import NaN


class Website:
    """Represents one of the websites that the scraper needs to visit, contains most of the datat that would
    hypothetically be collected from the website."""

    def __init__(self, name: str = '', brand: str = '', address: str = '',
                 latitude: float = NaN, longitude: float = NaN, price: float = NaN,
                 rating: float = NaN, rating_count: int = NaN):
        self.name = name
        self.brand = brand
        self.address = address
        self.latitude = latitude
        self.longitude = longitude
        self.price = price
        self.rating = rating
        self.rating_count = rating_count

    @property
    def location(self) -> tuple:
        return self.latitude, self.longitude

    def __str__(self) -> str:
        return "{}[{}](rating: {} ({} ratings)) at ({}, {}): ${:03,.2f}".format(
            self.name, self.brand, self.rating, self.rating_count, self.latitude, self.longitude,
            round(self.price, 2)
        )


class OilWebsite:
    """Represents the oil price website, just contains the price of oil, for now"""

    def __init__(self, barrel_price: float = NaN):
        self.barrel_price = barrel_price


class WebsiteDescriptor:
    """Represents one of the websites that the scraper needs to visit, contains all of the data that would
    hypothetically be collected from the website. Also adds the day of the year, and the year."""

    def __init__(self, w: Website = None, oil: OilWebsite = None):
        self.name = w.name if w else ''
        self.brand = w.brand if w else ''
        self.address = w.address if w else ''
        self.latitude = w.latitude if w else NaN
        self.longitude = w.longitude if w else NaN
        self.gallon_price = w.price if w else NaN
        self.rating = w.rating if w else NaN
        self.rating_count = w.rating_count if w else NaN
        self.barrel_price = oil.barrel_price if oil else NaN
        self.doy = dt.datetime.now().timetuple().tm_yday
        self.year = dt.date.today().year

    @property
    def location(self) -> tuple:
        return self.latitude, self.longitude

    def __str__(self) -> str:
        return "{}/{}: {}[{}](rating: {} ({} ratings)) at ({}, {}): ${:03,.2f}, barrel price: ${:03,.2f}".format(
            self.doy, self.year,
            self.name, self.brand, self.rating, self.rating_count, self.latitude, self.longitude,
            round(self.gallon_price, 2), round(self.barrel_price, 2)
        )

    @staticmethod
    def get_headers() -> list:
        return ["day_of_year", "year", "station_name", "latitude", "longitude", "gallon_price", "barrel_price"]

    def to_array(self) -> list:
        return [self.doy, self.year, self.name, self.latitude, self.longitude, self.gallon_price, self.barrel_price]

    def to_dict(self) -> dict:
        result = {}
        arr = self.to_array()
        for i, h in enumerate(self.get_headers()):
            result[h] = arr[i]
        return result


if __name__ == "__main__":
    print("Creating test components")
    w = Website("Shell", "shell", "I don't know", 1.34682, 135646843, 2.2)
    oil = OilWebsite(50.)

    print("Creating descriptor")
    desc = WebsiteDescriptor(w, oil)

    print(desc)
