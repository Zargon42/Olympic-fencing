public class Fencer
{
    public int ID { get; protected set; }
    public bool rightHandDominant { get; protected set; } 
    //public int countryOriginIndex { get; protected set; }
    public Country countryOrigin { get; protected set; }
    //public double timezone { get; protected set; }
    //public double rankingPoints { get; protected set; } //may be better to put in the bout as it's dependent on the match itself
    //public double[]? rankingHistory { get; protected set; }

    public Fencer(int ID, bool RHdom,  int countryIndex, List<Country> countries)
    {
        this.ID = ID;
        this.rightHandDominant = RHdom;
        //this.countryOriginIndex = countryIndex;
        this.countryOrigin = countries[countryIndex];
        //countryOrigin.timezone
        //this.rankingPoints = rankingPoints; 
        //this.rankingHistory = new[] {}
    }

}
