import math
import time

from selenium import webdriver
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.common.exceptions import TimeoutException, ElementNotInteractableException, StaleElementReferenceException

from WebsiteDescriptor import Website, WebsiteDescriptor

hyvee_home = 'https://www.hy-vee.com/'


def login(browser: webdriver.Firefox, username: str = '', password: str = '', timeout_delay: int = 30):
    """Navigates the browser to the hyvee homepage and logs in."""

    browser.get(hyvee_home)

    try:
        # Clicks on the login button
        WebDriverWait(browser,
                      timeout_delay).until(EC.presence_of_element_located((By.CSS_SELECTOR,
                                                                           'a.login-button'))).click()

        # Finds the submit button
        submit_button = WebDriverWait(browser,
                                      timeout_delay).until(EC.presence_of_element_located((By.CSS_SELECTOR,
                                                                                           'button[label="Log In"]')))

        browser.find_element_by_id('username').send_keys(username if username != '' else
                                                         input('Enter the email to user: '))

        browser.find_element_by_id('password').send_keys(password if password != '' else
                                                         input('Enter the password: '))
        submit_button.click()
    except TimeoutException as _:
        print('Somebody is already logged in.')

    return


def parse_price(price_string: str) -> float:
    """Converts the tons of different hyvee price strings into price numbers"""

    if price_string.find('/') >= 0:
        numbers = price_string.split('/')
        numerator = float(numbers[1].strip()[1:])
        denominator = float(numbers[0].strip())
        return math.ceil((numerator / denominator) * 100) / 100
    elif price_string.find('¢') >= 0:
        index = price_string.find('¢')
        number = float(price_string[:index].strip()) / 100  # Convert to cents
        return number
    elif price_string.find('each') >= 0:
        # Should be '$dollars.cents each'
        index = price_string.find('$')
        return float(price_string.split()[0][index + 1:].strip())
    elif price_string.find('per') >= 0:
        # Should be '$dollars.cents each'
        index = price_string.find('$')
        return float(price_string.split()[0][index + 1:].strip())
    else:
        # Should be $dollars.cents
        index = price_string.find('$')
        return float(price_string[index + 1:].strip())


def find_recent_purchases(browser: webdriver.Firefox, logged_in: bool, timeout_delay: int = 30) -> list:
    """Finds a list of recently purchased items and returns it."""

    # Navigates to the homepage, or logs in if not already.
    if logged_in:
        browser.get(hyvee_home)
    else:
        print('Logging in')
        login(browser, timeout_delay=timeout_delay)

    # Navigates to the previously bought items lists
    browser.get(hyvee_home + 'grocery/my-account/lists/frequent-purchases.aspx')

    print('Loading page')
    total_count = WebDriverWait(browser, timeout_delay).until(EC.presence_of_element_located((By.CSS_SELECTOR, 'span[id=ctl00_ContentPlaceHolder1_totalItems]')))

    while True:
        try:
            print('Trying to get total count')
            total_count = int(total_count.text)
            browser.get(hyvee_home + 'grocery/my-account/lists/frequent-purchases.aspx?pages={}'.format(total_count))
            print('Found {} items'.format(total_count))
            break
        except StaleElementReferenceException as _:
            print('Trying to find total count failed, trying again')
            continue

    items = WebDriverWait(browser,
                          timeout_delay).until(EC.presence_of_element_located((By.CSS_SELECTOR,
                                                                               'ul[id=ulProductList]')))

    items = items.find_elements_by_css_selector('li[id*=liProduct]')

    # print('Found {} products'.format(len(items)))
    products = []
    for i in items:
        product_name = i.find_element_by_css_selector('p.li-head').text
        price = parse_price(i.find_elements_by_css_selector('p[id*=pPrice]')[0].text)

        products.append(WebsiteDescriptor(Website(product_name, price)))

    return products
