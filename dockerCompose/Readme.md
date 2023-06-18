## Dump command

```sh
pg_dump -f dump_file  -U <user> -d DataMining
```

## Resotre

```sh
psql -U <user> DataMining < .\dump_file
```

Use cmd instead powerShell