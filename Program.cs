// Program.cs is where the app starts. It shows a menu in a loop until you quit.

var store = new EventStore("events.txt");

// The month we are currently looking at. Starts on today's month.
int year = DateTime.Today.Year;
int month = DateTime.Today.Month;

bool running = true;

while (running)
{
    Console.Clear();
    CalendarView.DrawMonth(year, month, store);

    Console.WriteLine();
    Console.WriteLine(" N) Next month     P) Previous month   T) Back to today");
    Console.WriteLine(" A) Add event      V) View a day       D) Delete event");
    Console.WriteLine(" U) Upcoming       Q) Quit");
    Console.Write("\n Choose: ");

    string choice = (Console.ReadLine() ?? "").Trim().ToUpper();

    switch (choice)
    {
        case "N":
            // AddMonths handles December -> January for us.
            DateTime next = new DateTime(year, month, 1).AddMonths(1);
            year = next.Year;
            month = next.Month;
            break;

        case "P":
            DateTime previous = new DateTime(year, month, 1).AddMonths(-1);
            year = previous.Year;
            month = previous.Month;
            break;

        case "T":
            year = DateTime.Today.Year;
            month = DateTime.Today.Month;
            break;

        case "A":
            AddEvent();
            break;

        case "V":
            ViewDay();
            break;

        case "D":
            DeleteEvent();
            break;

        case "U":
            Console.WriteLine("\n Upcoming events:");
            CalendarView.PrintEvents(store.GetUpcoming());
            Pause();
            break;

        case "Q":
            running = false;
            break;

        default:
            Console.WriteLine("\n Sorry, that's not a valid option.");
            Pause();
            break;
    }
}

Console.WriteLine("\n Goodbye!");


// ---------- Helper methods ----------

void Pause()
{
    Console.Write("\n Press Enter to continue...");
    Console.ReadLine();
}

// Ask the user for a day number in the month being shown. Returns null if invalid.
DateTime? AskForDay()
{
    int daysInMonth = DateTime.DaysInMonth(year, month);
    Console.Write($"\n Day of the month (1-{daysInMonth}): ");

    if (int.TryParse(Console.ReadLine(), out int day) && day >= 1 && day <= daysInMonth)
    {
        return new DateTime(year, month, day);
    }

    Console.WriteLine(" That is not a valid day.");
    return null;
}

void AddEvent()
{
    DateTime? day = AskForDay();
    if (day == null)
    {
        Pause();
        return;
    }

    Console.Write(" Time (HH:mm, e.g. 14:30, or press Enter for 09:00): ");
    string timeText = (Console.ReadLine() ?? "").Trim();
    if (timeText == "") timeText = "09:00";

    if (!TimeSpan.TryParse(timeText, out TimeSpan time) || time < TimeSpan.Zero || time >= TimeSpan.FromHours(24))
    {
        Console.WriteLine(" That is not a valid time.");
        Pause();
        return;
    }

    Console.Write(" Event title: ");
    string title = (Console.ReadLine() ?? "").Trim();
    if (title == "")
    {
        Console.WriteLine(" The title cannot be empty.");
        Pause();
        return;
    }

    // Replace the "|" character so it doesn't break our save file format.
    title = title.Replace("|", "/");

    store.Add(new CalendarEvent(day.Value.Date + time, title));
    Console.WriteLine(" Event added!");
    Pause();
}

void ViewDay()
{
    DateTime? day = AskForDay();
    if (day == null)
    {
        Pause();
        return;
    }

    Console.WriteLine($"\n Events on {day.Value:dddd, d MMMM yyyy}:");
    CalendarView.PrintEvents(store.GetForDay(day.Value));
    Pause();
}

void DeleteEvent()
{
    DateTime? day = AskForDay();
    if (day == null)
    {
        Pause();
        return;
    }

    List<CalendarEvent> events = store.GetForDay(day.Value);
    Console.WriteLine($"\n Events on {day.Value:d MMMM yyyy}:");
    CalendarView.PrintEvents(events);

    if (events.Count == 0)
    {
        Pause();
        return;
    }

    Console.Write("\n Number of the event to delete (or Enter to cancel): ");
    if (int.TryParse(Console.ReadLine(), out int number) && number >= 1 && number <= events.Count)
    {
        store.Remove(events[number - 1]);
        Console.WriteLine(" Event deleted.");
    }
    else
    {
        Console.WriteLine(" Nothing deleted.");
    }

    Pause();
}
