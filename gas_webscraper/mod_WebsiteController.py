from queue import Full
import datetime as dt

from WebsiteController import WebsiteController
from google_search_parser import generate_parsers


class WeeklyUpdateWebsiteController(WebsiteController):
    def update_parsers(self):
        self.parsers = generate_parsers(force_generate=True)

    def poll_websites(self) -> None:
        """Goes around to each website under its control and calls the 'parse()' method on it."""

        if dt.datetime.now().timetuple().tm_yday % 5 == 0:
            print("Updating parsers by force")
            self.update_parsers()

        for p in self.parsers:  # tqdm(self.parsers):
            parse_results = p.parse()
            if parse_results is not None:
                for pr in parse_results:
                    try:
                        self.Tx.put_nowait(pr.to_dict())
                    except Full as _:
                        print("Ran out of space in the parser queue, skipping\n{}".format(str(parse_results)))
            else:
                print("Result was None, skipping...")