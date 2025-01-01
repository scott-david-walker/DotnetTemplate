# Creating Migrations

Create migration:

```
dotnet ef migrations add "Init" --context ApplicationDbContext --project Persistence/ --startup-project Api/
```

Remove Migration:
```
dotnet ef migrations remove --context ApplicationDbContext --project Persistence/ --startup-project Api/
```

Update Database:
```
dotnet ef database update  --context ApplicationDbContext --project Persistence/ --startup-project Api/
```

Revert Database to specific migration:
```
dotnet ef database update "MIGRATION_NAME_WITHOUT_TIMESTAMP_HERE" --context ApplicationDbContext --project Persistence/ --startup-project Api/