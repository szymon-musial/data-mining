import requests
from html.parser import HTMLParser
from bs4 import BeautifulSoup
from datetime import datetime
from configparser import ConfigParser
import psycopg2

def config(filename='database.ini', section='postgresql'):
    # create a parser
    parser = ConfigParser()
    # read config file
    parser.read(filename)
  
    # get section, default to postgresql
    db = {}
    if parser.has_section(section):
        params = parser.items(section)
        for param in params:
            db[param[0]] = param[1]
    else:
        raise Exception('Section {0} not found in the {1} file'.format(section, filename))
  
    return db

def getConnection():
    """ Connect to the PostgreSQL database server """
    conn = None
    result = []
    try:
        # read connection parameters
        params = config()
  
        # connect to the PostgreSQL server
        print('Connecting to the PostgreSQL database...')
        conn = psycopg2.connect(**params)
        
        return conn
    
    except (Exception, psycopg2.DatabaseError) as error:
        print(error)

def cratePayload(day, month):
    return {
        "place": "Cracow,Poland",
        "day": day,
        "month": month,
        "city": "Cracow",
        "country": "Poland",
        "language": "polish",
        "canonical_url": "https://www.pogodajutro.com/europe/poland/malopolskie/cracow"
    }

headers = {
    'authority': '0cf0ddb661d6fd14a5a73422051492a8d1d7c00a',
    "Content-type": "application/x-www-form-urlencoded; charset=UTF-8",    
    }


class MyHTMLParser(HTMLParser):
    def handle_starttag(self, tag, attrs):
        print("Encountered a start tag:", tag)

    def handle_endtag(self, tag):
        print("Encountered an end tag :", tag)

    def handle_data(self, data):
        print("Encountered some data  :", data)

def getRequestFor(day, month):

    r = requests.post('https://www.pogodajutro.com/v1/past-weather/',  data=cratePayload(day, month), headers=headers)
    print(r.status_code)
    resp = r.json()
    
    rawHtmlInResp = resp['data']['years']['2023']['table']
    cleaned_string = rawHtmlInResp.replace('\n', '').replace('\t', '').replace('\r', '')

    with open('output.html', 'w', encoding='utf-8') as file:
        file.write(cleaned_string)
    
    raw_hours, degree, speed = htmlToTab(cleaned_string)

    #date_str = list(map(lambda x:  str(day) + ':' + str(month) + ':' + '2023' + ':' + x, hours))  
    hours =  list(map(lambda d: datetime.strptime(d, '%H:%M'), raw_hours))
    
    print(hours)
    print(degree)
    print(speed)

    return [[h, deg, spd] for h, deg, spd in zip(hours, degree, speed)]




def htmlToTab(html_doc):
    soup = BeautifulSoup(html_doc, 'html.parser')

    # Find the table element
    table = soup.find('table')

    # Extract the column names
    
    # this api use td not th in tables XD
    headers = [th.text for th in table.find('thead').find_all('td')]

    # Extract the data rows
    rows = []
    for tr in table.find('tbody').find_all('tr'):
        rows.append([td.text for td in tr.find_all('td')])

    # Print the data
    
    #print(headers)
 
    raw_hours = list(map(lambda x: x[:-1] ,headers))    
    speed = list(map(lambda x: x[:-4] ,rows[5]))
    degree = list(map(lambda x: x[:-1] ,rows[7]))
    
    return raw_hours, degree, speed

def insertIntoTable(day, month):    
    data = getRequestFor(day, month)

    conn = getConnection()
    cur = conn.cursor()
    
    for row in data:
        cur.execute('INSERT INTO airly."MissingWind" ("Day", "Month", "Hour", "WIND_BEARING", "WIND_SPEED") VALUES (%s, %s, %s, %s, %s)', (day, month, row[0].hour, float(row[1]), float(row[2])))
    
    conn.commit()
    conn.close()


from datetime import datetime, timedelta

start_date = datetime(2023, 3, 22)
end_date = datetime(2023, 4, 11)

# iteruj po każdej dacie między start_date i end_date
current_date = start_date
while current_date <= end_date:
    print(current_date.strftime("%d.%m.%Y"))
    
    # tutaj możesz dodać swoją logikę dla każdej daty     
    insertIntoTable(current_date.day, current_date.month)
    
    current_date += timedelta(days=1)


print('end')