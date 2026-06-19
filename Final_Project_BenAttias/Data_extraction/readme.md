# data extraction

extract relevant data values from each file for analyisis and run quries to check completeness of data
	check for missing values, whether data fit within expected ranges

## data format

### file 1 : bout info

- fencer_ID	
- opp_ID	
- fencer_age	
- opp_age	
- fencer_score	
- opp_score	
- winner_ID	
- fencer_curr_pts	
- opp_curr_pts	
- tournament_ID	= competition_ID
- pool_ID	
- upset	
- date

### file 2 : fencer information

- id			int
- name			string
- country_code	string
- country		string
- hand			string (Left or Right)
- age			int
- url			string
- date_accessed	DateTime

### file 3 : competition info

- competition_ID	
- season	
- name	
- category	
- country	
- start_date	
- end_date	
- weapon	
- gender	
- timezone	
- url	
- unique_ID	
- missing_results_flag

### file 4 output

- fencer_ID	
- F1_country 	
- F1_Base_Timezone (hours away from gmt)	
- F1_hand (1=RH, 0=LH)	
- opp_ID	
- F2_country	
- F2_Base_timezone	
- F1_Hand	
- fencer_age	
- opp_age	
- fencer_score	
- opp_score	
- winner_ID	
- fencer_curr_pts	
- F1_Historical_points	
- opp_curr_pts	
- F2_Historical_Pooints	
- tournament_ID	
- Country	
- Timezone	
- pool_ID	
- upset	
- date		
- Delta	
- F1_Timezone_Diff	
- F2_Timezone_Diff	
- F1_Home	
- F2_Home		
- notes
