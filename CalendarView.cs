// Everything to do with DRAWING the calendar on screen.
public static class CalendarView
{
    public static void DrawMonth(int year, int month, EventStore store)
    {
        DateTime firstOfMonth = new DateTime(year, month, 1);
        int daysInMonth = DateTime.DaysInMonth(year, month);
        int startColumn = (int)firstOfMonth.DayOfWeek; // Sunday = 0 ... Saturday = 6
        DateTime today = DateTime.Today;

        // Which days of this month have at least one event?
        var daysWithEvents = store.GetForMonth(year, month)
                                  .Select(e => e.Date.Day)
                                  .ToHashSet();

        Console.WriteLine();
        Console.WriteLine($"      {firstOfMonth:MMMM yyyy}");
        Console.WriteLine(" Su  Mo  Tu  We  Th  Fr  Sa");

        // Print blank spaces until the first day lines up under the right weekday.
        for (int i = 0; i < startColumn; i++)
        {
            Console.Write("    ");
        }

        for (int day = 1; day <= daysInMonth; day++)
        {
            bool isToday = (year == today.Year && month == today.Month && day == today.Day);
            bool hasEvent = daysWithEvents.Contains(day);

            // Today is shown in [brackets], days with events get a * after them.
            string text = isToday ? $"[{day,2}]" : $" {day,2}{(hasEvent ? "*" : " ")}";
            if (isToday) Console.ForegroundColor = ConsoleColor.Cyan;
            else if (hasEvent) Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write(text);
            Console.ResetColor();

            // Start a new line after Saturday.
            if ((startColumn + day) % 7 == 0)
            {
                Console.WriteLine();
            }
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(" [ ] = today    * = has events");
    }

    public static void PrintEvents(List<CalendarEvent> events)
    {
        if (events.Count == 0)
        {
            Console.WriteLine("  (no events)");
            return;
        }

        for (int i = 0; i < events.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {events[i]}");
        }
    }
}
