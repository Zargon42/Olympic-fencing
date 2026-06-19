using Accord.Statistics.Models.Fields.Features;
using System;



public class InputOutput
{
    public double[][] inputs { get; protected set; }
    public double[][] outputs { get; protected set; }

    public InputOutput(Bout[] dataset)
    {
        try
        {
            inputs = new double[dataset.Length][];
            for (int i = 0; i < dataset.Length; i++)
            {
                inputs[i] = new double[]
                {
                //fencer 1
                dataset[i].F1_currentAge,
                dataset[i].F1_currentRankingPoints,
                dataset[i].fencer1.countryOrigin.timezone,
                Math.Abs(dataset[i].F1_Timezone_Diff), //takes the magnitude of the difference between timezones instead of the raw value.
                dataset[i].F1_rightHand,                
                
                //fencer 2
                dataset[i].F2_currentAge,
                dataset[i].F2_currentRankingPoints,
                dataset[i].fencer2.countryOrigin.timezone,
                Math.Abs(dataset[i].F2_Timezone_Diff),                 
                dataset[i].F2_rightHand,



                //difference data
                dataset[i].F1_currentAge - dataset[i].F2_currentAge,
                dataset[i].F1_currentRankingPoints - dataset[i].F2_currentRankingPoints,
                dataset[i].F1_Timezone_Diff - dataset[i].F2_Timezone_Diff
                };
                
                /*
                //feature normalisation
                //min-max scaling                
                double min = inputs[i].Min();
                double max = inputs[i].Max();
 

                double range = max - min;

                double[] normalizedFeature_MinMax = inputs[i].Select(x => (2*(x - min) / range) -1).ToArray(); //this is only normalising within each array
                
                

                //standardisation
                double mean = inputs[i].Average();
                double standardDeviation = Math.Sqrt(inputs[i].Select(x => Math.Pow(x - mean, 2)).Average());

                double[] normalizedFeature_Stand= inputs[i].Select(x => (x - mean) / standardDeviation).ToArray();

                inputs[i] = normalizedFeature_MinMax;
                */
                
            }

            outputs = new double[dataset.Length][];
            for (int i = 0; i < dataset.Length; i++)
            {
                outputs[i] = new double[]
                {
                dataset[i].F1_score,
                dataset[i].F2_score,
                dataset[i].delta
                };
            }


            //print sample
            int n = 5;
            Console.WriteLine("F1_Age \t F1_RKPts \t F1_timeZn \t F1_TZDif \t F1_RH? \t F2_Age \t F2_RKPts \t F2_timeZn \t F2_TZDif \t F2_RH?");

            Console.WriteLine("Inputs:");
            for (int i = 0; i < n && i < inputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var input in inputs[i])
                {
                    Console.Write(input + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Outputs: ");
            for (int i = 0; i < n && i < outputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var output in outputs[i])
                {
                    Console.Write(output + "\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failure to seperate the dataset into input and output values");
            Console.WriteLine(ex.ToString());
        }

    }
    /*
    public InputOutput(Bout[] dataset, bool categorical)
    {
        try
        {
            inputs = new double[dataset.Length][];
            for (int i = 0; i < dataset.Length; i++)
            {
                inputs[i] = new double[]
                {
                //fencer 1
                dataset[i].F1_currentAge,
                dataset[i].F1_currentRankingPoints,
                dataset[i].F1_timezone,
                dataset[i].F1_Timezone_Diff,
                dataset[i].F1_rightHand,                
                
                //fencer 2
                dataset[i].F2_currentAge,
                dataset[i].F2_currentRankingPoints,
                dataset[i].F2_Timezone_Diff,
                dataset[i].F2_timezone,
                dataset[i].F2_rightHand
                };

                /*
                //feature normalisation
                //min-max scaling                
                double min = inputs[i].Min();
                double max = inputs[i].Max();
 

                double range = max - min;

                double[] normalizedFeature_MinMax = inputs[i].Select(x => (2*(x - min) / range) -1).ToArray(); //this is only normalising within each array
                
                

                //standardisation
                double mean = inputs[i].Average();
                double standardDeviation = Math.Sqrt(inputs[i].Select(x => Math.Pow(x - mean, 2)).Average());

                double[] normalizedFeature_Stand= inputs[i].Select(x => (x - mean) / standardDeviation).ToArray();

                inputs[i] = normalizedFeature_MinMax;
                
            }

            outputs = new double[dataset.Length][];
            for (int i = 0; i < dataset.Length; i++)
            {
                outputs[i] = new double[]
                {
                dataset[i].F1_score,
                dataset[i].F2_score,
                dataset[i].delta
                };
            }


            //print sample
            int n = 5;
            Console.WriteLine("F1_Age \t F1_RKPts \t F1_timeZn \t F1_TZDif \t F1_RH? \t F2_Age \t F2_RKPts \t F2_timeZn \t F2_TZDif \t F2_RH?");

            Console.WriteLine("Inputs:");
            for (int i = 0; i < n && i < inputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var input in inputs[i])
                {
                    Console.Write(input + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Outputs: ");
            for (int i = 0; i < n && i < outputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var output in outputs[i])
                {
                    Console.Write(output + "\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failure to seperate the dataset into input and output values");
            Console.WriteLine(ex.ToString());
        }
    }*/
}


public class IONormalised : InputOutput
{

    public IONormalised(Bout[] dataset, string normMethod) : base(dataset)
    {
        try
        {
            //feature normalisation
            int numRows = inputs.Length;
            int numCols = inputs[0].Length;

            // Calculate column-wise statistics
            double[] columnMeans = new double[numCols];
            double[] columnStdDevs = new double[numCols];
            double[] columnMins = new double[numCols];
            double[] columnMaxs = new double[numCols];


            for (int col = 0; col < numCols; col++)
            {
                double sum = 0.0;
                double sumSquares = 0.0;
                double min = double.MaxValue;
                double max = double.MinValue;

                for (int row = 0; row < numRows; row++)
                {
                    double value = inputs[row][col];
                    sum += value;
                    sumSquares += value * value;

                    if (value < min)
                        min = value;

                    if (value > max)
                        max = value;
                }

                double mean = sum / numRows;
                double variance = (sumSquares / numRows) - (mean * mean);
                double stdDev = Math.Sqrt(variance);

                columnMeans[col] = mean;
                columnStdDevs[col] = stdDev;
                columnMins[col] = min;
                columnMaxs[col] = max;
                
                //check for outliers
                Console.WriteLine("Column {0} has a mean of {1}, a Standard deviation of {2} a min value of {3} and a max value of {4}",
                    col, columnMeans[col], columnStdDevs[col], columnMins[col], columnMaxs[col]);
            }

            // Min-Max Scaling
            if (normMethod.Equals("MinMax")) 
            {
                for (int col = 0; col < numCols; col++)
                {
                    double min = columnMins[col];
                    double max = columnMaxs[col];
                    double range = max - min;

                    for (int row = 0; row < numRows; row++)
                    {
                        inputs[row][col] = (inputs[row][col] - min) / range;
                    }
                }
                Console.WriteLine("Min-Max normalisation method applied");
            }


            // Standardization
            else if (normMethod.Equals("Standard"))
            {
                for (int col = 0; col < numCols; col++)
                {
                    double mean = columnMeans[col];
                    double stdDev = columnStdDevs[col];

                    for (int row = 0; row < numRows; row++)
                    {
                        inputs[row][col] = (inputs[row][col] - mean) / stdDev;
                    }
                }
                Console.WriteLine("Standardization normalisation method applied");
            }
            else if (normMethod.Equals("None"))
            {
                Console.WriteLine("No normalisation method applied");
            }
            else
            {
                Console.WriteLine("Normalisation method invalid " + normMethod + " is not an understood method");
                Console.WriteLine("No normalisation method applied");
            }

            //print sample
            int n = 5;
            Console.WriteLine("F1_Age \t F1_RKPts \t F1_timeZn \t F1_TZDif \t F1_RH? \t F2_Age \t F2_RKPts \t F2_timeZn \t F2_TZDif \t F2_RH?");

            Console.WriteLine("Inputs:");
            for (int i = 0; i < n && i < inputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var input in inputs[i])
                {
                    Console.Write(input + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Outputs: ");
            for (int i = 0; i < n && i < outputs.Length; i++)
            {
                Console.WriteLine("Bout {0}: ", i);
                foreach (var output in outputs[i])
                {
                    Console.Write(output + "\t");
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failure to seperate the dataset into input and output values");
            Console.WriteLine(ex.ToString());
        }
    }

}
