/*
 *
 *fencer_ID	F1_country 	F1_Base_Timezone (hours away from gmt)	F1_hand (1=RH, 0=LH)	
 *opp_ID	F2_country	F2_Base_timezone	F1_Hand	fencer_age	opp_age	fencer_score	
 *opp_score	winner_ID	fencer_curr_pts	F1_Historical_points	opp_curr_pts	
 *F2_Historical_Pooints	tournament_ID	Country	Timezone	pool_ID	upset	date		
 *Delta	F1_Timezone_Diff	F2_Timezone_Diff	F1_Home	F2_Home		notes
 *
 *Fencer 1, fencer 2, fencer 1 score, fencer 2 score, tournament
 *
 *methods:
 *contstuctor, 

 */

public class Bout
{
    //fencer 1
    public Fencer fencer1 { get; protected set; }  
    public double F1_rightHand { get; protected set; }
    
    public int F1_currentAge { get; protected set; }
    public int F1_score { get; protected set; }
    public double F1_currentRankingPoints { get; protected set; }

    //fencer 2
    public Fencer fencer2 { get; protected set; }
    public double F2_rightHand { get; protected set; }
   
    public int F2_currentAge { get; protected set; }
    public int F2_score { get; protected set; }
    public double F2_currentRankingPoints { get; protected set; }

    //bout
    public int boutWinner { get; protected set; }
    public bool upset { get; protected set; }
    public Tournament tournament { get; protected set; }    
    public DateTime date { get; protected set; }

    //derived properties
    public int delta { get; protected set; }
    public double F1_Timezone_Diff { get; protected set; }
    public double F2_Timezone_Diff { get;protected set; }
    public bool F1_Home { get; protected set; }
    public bool F2_Home { get;protected set; }


    //constructors
    public Bout(int F1_index, int F1_currentAge, int F1_score, double F1_currentRankingPoints,
        int F2_index, int F2_currentAge, int F2_score, double F2_currentRankingPoints,
        int winner, bool upset, int tournIndex, DateTime date,
        List<Fencer> fencers, List<Tournament> tournaments)
    {
        //fencer 1
        this.fencer1 = fencers[F1_index];
        if (fencer1.rightHandDominant == true)
        {
            this.F1_rightHand = 1;
        }
        else
        {
            this.F1_rightHand = 0;
        }
        
        this.F1_currentAge = F1_currentAge;
        this.F1_score = F1_score;
        this.F1_currentRankingPoints = F1_currentRankingPoints;
        //fencer 2
        this.fencer2 = fencers[F2_index];
        if (fencer2.rightHandDominant == true)
        {
            this.F2_rightHand = 1;
        }
        else
        {
            this.F2_rightHand = 0;
        }
        
        this.F2_currentAge = F2_currentAge;
        this.F2_score = F2_score;
        this.F2_currentRankingPoints = F2_currentRankingPoints;
        //bout specific
        this.boutWinner = winner;
        this.upset = upset;
        this.tournament = tournaments[tournIndex];
        this.date = date;
        //derivations
        this.delta = F1_score - F2_score; // no.16 [15]
        this.F1_Timezone_Diff = fencer1.countryOrigin.timezone - tournament.eventCountry.timezone;
        this.F2_Timezone_Diff = fencer2.countryOrigin.timezone - tournament.eventCountry.timezone;
        this.F1_Home = fencer1.countryOrigin.Equals(tournament.eventCountry);
        this.F2_Home = fencer2.countryOrigin.Equals(tournament.eventCountry);
    }
}