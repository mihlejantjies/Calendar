# Calendar (C#)

A beginner-friendly calendar for the terminal. Events are saved to `events.txt`.

## Run it
1. Install the .NET 8 SDK: https://dotnet.microsoft.com/download
2. In this folder run:

       dotnet run

## Features
- Month view with today highlighted and event days marked with `*`
- Next / previous month, jump back to today
- Add, view and delete events
- Upcoming events list
- Events saved to a text file between runs

## Files (read them in this order)
| File | What it teaches |
|---|---|
| `CalendarEvent.cs` | Classes, properties, constructors |
| `EventStore.cs` | Lists, LINQ (`Where`), reading/writing files |
| `CalendarView.cs` | Loops, `DateTime`, formatting output |
| `Program.cs` | Menu loop, `switch`, user input, methods |

## Ideas to extend it
- Edit an existing event
- Add event categories or colours
- Search events by title
- Store events as JSON with `System.Text.Json`
- Rebuild the UI in WPF
