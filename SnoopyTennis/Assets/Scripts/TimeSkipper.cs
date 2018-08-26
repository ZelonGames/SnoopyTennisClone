
public class TimeSkipper
{
    public int timeSkips;
    private int timeCounter;

    public TimeSkipper(int timeSkips)
    {
        this.timeSkips = timeSkips;
    }

    public void Update()
    {
        timeCounter++;
    }

    public void Reset()
    {
        timeCounter = 0;
    }

    public bool Done
    {
        get
        {
            return timeCounter % timeSkips == 0;
        }
    }
}
