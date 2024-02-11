# Weather Scraper

This is a console application that scrapes the website "https://www.ach-du-schan.de/" to retrieve local weather data. The user can specify whether they want to retrieve the temperature and/or humidity from the website. The retrieved data is then printed to the console in order to be piped to other programs for home automation purposes.

## Usage

The application accepts two optional command line arguments:

- `-t` or `--temperature`: Use this option to retrieve and print the temperature.
- `-h` or `--humidity`: Use this option to retrieve and print the humidity.

You can use these options separately or together. For example:

```bash
# To get the temperature
dotnet run -- -t

# To get the humidity
dotnet run -- -h

# To get both the temperature and humidity
dotnet run -- -t -h

Please ensure you have .NET Core installed to run this application.
```