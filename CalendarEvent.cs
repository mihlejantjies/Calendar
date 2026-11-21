public class CalendarEvent
{
    public DateTime Date { get; set; }          // the day (and time) of the event
    public string Title { get; set; } = "";     // e.g. "Dentist appointment"

    // A "constructor" runs when you create a new CalendarEvent.
    public CalendarEvent(DateTime date, string title)
    {
        Date = date;
        Title = title;
    }

    // How the event is shown on screen.
    public override string ToString()
    {
        return $"{Date:yyyy-MM-dd HH:mm}  {Title}";
    }
}
