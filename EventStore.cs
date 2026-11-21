// Handles keeping the list of events and saving/loading them from a file,
// so your events are still there the next time you run the program.
public class EventStore
{
    private readonly string _filePath;
    private readonly List<CalendarEvent> _events = new List<CalendarEvent>();

    public EventStore(string filePath)
    {
        _filePath = filePath;
        Load();
    }

    // Add an event, keep the list sorted by date, and save.
    public void Add(CalendarEvent calendarEvent)
    {
        _events.Add(calendarEvent);
        _events.Sort((a, b) => a.Date.CompareTo(b.Date));
        Save();
    }

    // Remove an event and save.
    public void Remove(CalendarEvent calendarEvent)
    {
        _events.Remove(calendarEvent);
        Save();
    }

    // All events in a given month (used to mark days on the calendar).
    public List<CalendarEvent> GetForMonth(int year, int month)
    {
        return _events.Where(e => e.Date.Year == year && e.Date.Month == month).ToList();
    }

    // All events on one specific day.
    public List<CalendarEvent> GetForDay(DateTime day)
    {
        return _events.Where(e => e.Date.Date == day.Date).ToList();
    }

    // Every event from today onwards.
    public List<CalendarEvent> GetUpcoming()
    {
        return _events.Where(e => e.Date >= DateTime.Now.Date).ToList();
    }

    // Save: one event per line, written as  2026-11-21 14:30|Dentist
    private void Save()
    {
        var lines = _events.Select(e => $"{e.Date:yyyy-MM-dd HH:mm}|{e.Title}");
        File.WriteAllLines(_filePath, lines);
    }

    // Load: read each line back and turn it into a CalendarEvent.
    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            return; // first run, nothing saved yet
        }

        foreach (string line in File.ReadAllLines(_filePath))
        {
            string[] parts = line.Split('|', 2);
            if (parts.Length == 2 &&
                DateTime.TryParseExact(parts[0], "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out DateTime date))
            {
                _events.Add(new CalendarEvent(date, parts[1]));
            }
        }

        _events.Sort((a, b) => a.Date.CompareTo(b.Date));
    }
}
