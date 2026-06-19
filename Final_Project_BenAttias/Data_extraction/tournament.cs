public class Tournament
{
    public int Id { get; protected set; } // tournament id
    public string? title { get; protected set; }
    public DateTime startDate { get; protected set; }
    public DateTime endDate { get; protected set; }
    public Country eventCountry { get; protected set; }
    public string category { get; protected set; }
    public string weapon { get; protected set; }
    public bool womens { get; protected set; }
    public string uniqueId { get; protected set; }


    //constructors
    public Tournament(int id, string title, DateTime startDate, DateTime endDate, int countryIndex, 
        string category, string weapon, bool womens, string uId,
        List<Country> countries)
    {
        Id = id;
        this.title =   title;
        this.startDate = startDate; 
        this.endDate = endDate;
        this.eventCountry = countries[countryIndex];
        this.category = category;
        this.weapon = weapon;
        this.womens = womens;
        this.uniqueId = uId;

    }
}