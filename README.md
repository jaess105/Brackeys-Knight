# Brackeys-Knight

Simple game written in [GODOT](https://godotengine.org/) after the [beginners tutorial](https://www.youtube.com/watch?v=LOhfqjmasi0&t=788s) from [Brackeys](https://www.youtube.com/@Brackeys).


## Development

### Database

To update the database run:

```bash
cd scripts/Projects/Database
dotnet ef migrations add <MigrationName> 
```

This will create a new migration file which will be automatically worked in when the game is started
by the player or during development.

