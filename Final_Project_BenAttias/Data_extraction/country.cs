public class Country
{
    public double timezone { get; protected set; } = 0;
    public string name { get; protected set; }
    public string ID_code { get; protected set; }

    public Country (string ID, string name, double timezone)
    {
        this.ID_code = ID;
        this.name = name;
        this.timezone = timezone;
    }

    public Country (string ID, string name)
    {
        this.ID_code = ID;
        this.name = name;
        this.timezone = 0;
    }
}