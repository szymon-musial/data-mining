## Temat
3. Przewidywanie poziomu zanieczyszczenia powietrza w Krakowie i okolicach na podstawie danych z czujników Airly i sytuacji pogodowe


## Linki

- https://docs.google.com/document/d/1A4vf6-44v6pEoVfWlZY5T6w5sXWa10URgPM9IqERqiM/edit

## Architektura

```mermaid
flowchart TD
    P[powietrze.gios.gov.pl]
    D[danepubliczne.imgw.pl]
    A[Airly]
    R[wetter.com]
    
    Db[("Database: PostgreSQL")]

    Py[Notebook python]


    P -- Zip: Xlsx fa:fa-file-excel --> Db
    D -- Zip: Csv fa:fa-file-csv --> Db

    A --REST fa:fa-wifi --> Db
    R --REST fa:fa-wifi --> Db

    Db --TCP fa:fa-wifi --> Py
```
