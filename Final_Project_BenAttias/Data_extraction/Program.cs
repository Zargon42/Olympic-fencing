
using Accord.Statistics.Models.Regression.Linear; // Accord Library for Linear regression
using Accord.Math.Optimization.Losses; // Accord Library for evaluation metrics
using System.Globalization;



public class Program
{
    static public List<Fencer> fencers = new List<Fencer>();
    static public List<Country> countries = new List<Country>();
    static public List<Tournament> tournaments = new List<Tournament>();
    static public List<Bout> bouts = new List<Bout>();
    static public int trainSize { get; protected set; }
    static public Bout[] trainSet { get; protected set; }
    static public Bout[] testSet { get; protected set; }

    static void Main(string[] args)
    {        
        //CultureInfo.CurrentCulture = CultureInfo.InvariantCulture; // Set the culture to InvariantCulture for consistent decimal parsing
        //CultureInfo customFormatProvider = new CultureInfo(CultureInfo.InvariantCulture.Name);
        //customFormatProvider.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
        Console.WriteLine("starting");
        LoadCountries("C:/Users/bensa/OneDrive/Documents/Coursi singoli/SCOOP/Project/Datasets/Womens foil olympics/Countries.csv");
        LoadFencers("C:/Users/bensa/OneDrive/Documents/Coursi singoli/SCOOP/Project/Datasets/Womens foil olympics/all_womens_foil_fencer_bio_data_May_13_2021_cleaned.csv");
        LoadTournaments("C:/Users/bensa/OneDrive/Documents/Coursi singoli/SCOOP/Project/Datasets/Womens foil olympics/all_womens_foil_tournament_data_May_13_2021_cleaned.csv");
        
        LoadBouts("C:/Users/bensa/OneDrive/Documents/Coursi singoli/SCOOP/Project/Datasets/Womens foil olympics/all_womens_foil_bout_data_May_13_2021_cleaned.csv");
        PrintBouts(5);
        
        trainSize = bouts.Count * 80 / 100;
        trainSet = new Bout[trainSize];
        testSet = new Bout[bouts.Count - trainSize];
        RandomizeBouts();
        MultiVarLinearRegress("MinMax");
        MultiVarLinearRegress("Standard");
        MultiVarLinearRegress("None");
        
    }


    static public void PrintBouts(int n)
    {
        //loads a sample of n bouts for manual checking purposes
        Console.WriteLine("Index \t" + string.Join("\t", typeof(Bout).GetProperties().Select(p => p.Name)));

        for (int i = 0; i < n && i < bouts.Count; i++)
        {
            Console.Write(i + "\t");
            foreach (var property in typeof(Bout).GetProperties())
            {
                Console.Write(property.GetValue(bouts[i]) + "\t");
            }
            Console.WriteLine();
        }
    }

    //make train and test arrays
    static public void RandomizeBouts()
    {
        
        /// Shuffle the list randomly using Fisher-Yates shuffle algorithm
        Random rng = new Random();
        int n = bouts.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = bouts[k];
            bouts[k] = bouts[n];
            bouts[n] = value;
        }

        /// Split the shuffled list into arrays with relevant sizes
        //int arraySize = bouts.Count / 3;
        /*
        trainSet = bouts.Take(trainSize).ToArray();
        testSet = bouts.Skip(trainSize).Take(bouts.Count-trainSize).ToArray();
        object[] array3 = myboutsist.Skip(arraySize * 2).ToArray();
        */

        //divides randomized bout list into train and test arrays
        if (bouts.ToArray() != null)
        {
            Array.Copy(bouts.ToArray(), 0, trainSet, 0, trainSize);
            Array.Copy(bouts.ToArray(), trainSize, testSet, 0, bouts.Count - trainSize);
        }
        else
        {
            Console.WriteLine("the array bouts.array() is empty");
        }
        


        //checks
        Console.WriteLine("TrainSize is equal to: " + trainSize);
        Console.WriteLine("Training set has length of: " + trainSet.Length + " Testing set has length of: " + testSet.Length);

    }

 
    static public void MultiVarLinearRegress(string normType)
    {
        try
        {
            Console.WriteLine("==========");
            Console.WriteLine("MultiVar LINEAR REGRESSION");
            Console.WriteLine("==========");
            IONormalised train = new IONormalised(trainSet, normType);
            IONormalised test = new IONormalised(testSet, normType);

            //InputOutput train = new InputOutput(trainSet);
            //InputOutput test = new InputOutput(testSet);

            Console.WriteLine("train set length \n inputs: " + train.inputs.Length + "\n outputs: " +  train.outputs.Length);
            Console.WriteLine("test set length \n inputs: " + test.inputs.Length + "\n outputs: " + test.outputs.Length);

            // Use Ordinary Least Squares to create the regression
            OrdinaryLeastSquares ols = new OrdinaryLeastSquares()
            {
                UseIntercept = true
            };



            // Computation of multivariate linear regression:
            MultivariateLinearRegression regression = ols.Learn(train.inputs, train.outputs);

            // Prediction values on both training and test set
            double[][] predictions_train = regression.Transform(train.inputs);
            double[][] predictions_test = regression.Transform(test.inputs);

            Console.WriteLine("predictions check:");
            Console.WriteLine("predictions size: " + predictions_test.Length);
            Console.WriteLine("test outputs length: " + test.outputs.Length);
            for (int i = 0; i < 5 && i < predictions_test.Length; i++)
            {
                Console.WriteLine("Array {0}: ", i);
                foreach (var prediction in predictions_test[i])
                {
                    Console.Write(prediction + "\t");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < 5 && i < predictions_test.Length; i++)
            {
                Console.WriteLine("actual result {0}: ", i);
                foreach (var real in test.outputs[i])
                {
                    Console.Write(real + "\t");
                }
                Console.WriteLine();
            }

            // The prediction error
            //on training set
            double error_train = new SquareLoss(train.outputs).Loss(predictions_train);
            //on test set
            double error_test = new SquareLoss(test.outputs).Loss(predictions_test);

            //print results
            //intercepts
            double[] learnedIntercepts = regression.Intercepts;
            double[][] learnedCoeficients = regression.Weights;
            Console.WriteLine("Learned intercepts of regression: ");
            foreach (double element in learnedIntercepts)
            {
                Console.WriteLine(element);
            }
            Console.WriteLine("Learned coefficients of regression: ");
            for (int row = 0; row < learnedCoeficients.Length; row++)
            {
                Console.WriteLine("column: " + row);
                foreach (double element in learnedCoeficients[row])
                {
                    Console.WriteLine(element);
                }
                    
            }
            // Compute the coefficient of determination r²
            //on train set
            double[] r2_train = regression.CoefficientOfDetermination(train.inputs, train.outputs);
            double[] r2_train_alt = regression.CoefficientOfDetermination(train.inputs, train.outputs, adjust: true);

            //on test set
            double[] r2_test = regression.CoefficientOfDetermination(test.inputs, test.outputs);
            double[] r2_test_alt = regression.CoefficientOfDetermination(test.inputs, test.outputs, adjust: true);


            // Compute the coefficient of determination r² (alternative way)
            //double[] alternative_r2_train = regression.CoefficientOfDetermination(feat_x_train, feat_y_train, adjust: true);

            // Performance on the train set
            Console.WriteLine("*** Performance on train set ***");
            Console.WriteLine("error: " + error_train);
            Console.WriteLine("r2s on training data");
            foreach (double element in r2_train)
            {
                Console.WriteLine(element);
            }
            Console.WriteLine("alternative r2s on train data");
            foreach (double element in r2_train_alt)
            {
                Console.WriteLine(element);
            }
            // Performance on the test set
            Console.WriteLine("*** Performance on test set ***");
            Console.WriteLine("error: " + error_test);
            Console.WriteLine("r2s on test data");

            foreach (double element in r2_test)
            {
                Console.WriteLine(element);
            }
            Console.WriteLine("alternative r2s on test data");
            foreach (double element in r2_test_alt)
            {
                Console.WriteLine(element);
            }


            //Console.WriteLine("r2 (alternative): " + alternative_r2_train);
            //Console.WriteLine("the loss function results on the training data were: " + error);
        }
        catch(InvalidOperationException e)
        {
            Console.WriteLine("Error performing operation. " + e.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected ERROR: failed to perform multi var regression on the dataset");
            Console.WriteLine(ex.Message);
        }
    }
    
    /*
    static public void LinearRegression()
    {
        try
        {
            Console.WriteLine("==========");
            Console.WriteLine("LINEAR REGRESSION");
            Console.WriteLine("==========");
            InputOutput train = new InputOutput(trainSet);
            InputOutput test = new InputOutput(testSet);

            // Use Ordinary Least Squares (OLS)
            OrdinaryLeastSquares ols = new OrdinaryLeastSquares();

            // Use OLS to learn the simple linear regression
            SimpleLinearRegression regression = ols.Learn(train.inputs, train.outputs);


            // We can also extract the slope and the intercept term from the build model
            double learned_slope = regression.Slope;
            double learned_intercept = regression.Intercept;

            Console.WriteLine("\nLearned relationship: y = {0} * x + {1}", learned_slope, learned_intercept);


            // Now test the performance of the model!

            // First, compute the output (y) for a given input (x)
            // in this case we will use the test data
            double[][] predicted_y = regression.Transform(train.inputs);


            // Compute the squared error (using the SquareLoss class)
            double error = new SquareLoss(feat_y_test).Loss(predicted_y);

            // Compute the coefficient of determination r²
            double r2 = new RSquaredLoss(feat_y_test.Length, feat_y_test).Loss(predicted_y);

            // Compute the coefficient of determination r² (alternative way)
            double alternative_r2 = regression.CoefficientOfDetermination(feat_x_test, feat_y_test, adjust: true);

            Console.WriteLine("error: " + error);
            Console.WriteLine("r2: " + r2);
            Console.WriteLine("r2 (alternative): " + alternative_r2);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected ERROR: failed to perform linear regression on the dataset");
            Console.WriteLine(ex.Message);
        }
    }
    */

    /*
    static public void PolinomialRegressionOnDelta(int testDegree)
    {
        try
        {
            Console.WriteLine("==========");
            Console.WriteLine("Polynomial REGRESSION");
            Console.WriteLine("==========");
            InputOutput train = new InputOutput(trainSet);
            InputOutput test = new InputOutput(testSet);

            //feature engineering
            double[][] trainInputs_poly = new double[train.inputs.Length][];
            for (int i  = 0; i < train.inputs.)




            Console.WriteLine("train set length \n inputs: " + train.inputs.Length + "\n outputs: " + train.outputs.Length);
            Console.WriteLine("test set length \n inputs: " + test.inputs.Length + "\n outputs: " + test.outputs.Length);


            // Use Least Squares for polynomial regression
            PolynomialLeastSquares pls = new PolynomialLeastSquares();
            pls.Degree = testDegree; // set degree of polynom

            PolynomialRegression poly_regression = pls.Learn(train.inputs, train.outputs[2]);

            // Extract the coeffiecients and the degree from the build model
            double[] learned_coefficient = poly_regression.Weights; // coefficient of x
            double learned_intercept = poly_regression.Intercept; // intercept
            double degree = poly_regression.Degree; // degree of polynom

            Console.WriteLine("Degree: " + degree);

            Console.WriteLine("Learned coefficient: " +
                        String.Join(" ", learned_coefficient) + " "
                        + learned_intercept);


            // We can obtain predictions using
            double[][] predictions_train = regression.Transform(train.inputs);
            double[][] predictions_test = regression.Transform(test.inputs);

            Console.WriteLine("predictions check:");
            Console.WriteLine("predictions size: " + predictions_test.Length);
            Console.WriteLine("test outputs length: " + test.outputs.Length);
            for (int i = 0; i < 5 && i < predictions_test.Length; i++)
            {
                Console.WriteLine("Array {0}: ", i);
                foreach (var prediction in predictions_test[i])
                {
                    Console.Write(prediction + "\t");
                }
                Console.WriteLine();
            }


            // The prediction error
            //on training set
            double error_train = new SquareLoss(train.outputs).Loss(predictions_train); // 0
            //on test set
            double error_test = new SquareLoss(test.outputs).Loss(predictions_test);

            //print results
            //intercepts
            double[] learnedIntercepts = regression.Intercepts;
            Console.WriteLine("Learned intercepts of regression: ");
            foreach (double element in learnedIntercepts)
            {
                Console.WriteLine(element);
            }
            // Compute the coefficient of determination r²
            //on train set
            double[] r2_train = new RSquaredLoss(train.outputs.Length, train.outputs).Loss(predictions_train);
            //on test set

            double[] r2_test = new RSquaredLoss(test.outputs.Length, test.outputs).Loss(predictions_test);


            // Compute the coefficient of determination r² (alternative way)
            //double alternative_r2_train = poly_regression.CoefficientOfDetermination(feat_x_train, feat_y_train, adjust: true);

            // Performance on the train set
            Console.WriteLine("*** Performance on train set ***");
            Console.WriteLine("error: " + error_train);
            Console.WriteLine("r2s on training data");
            foreach (double element in r2_train)
            {
                Console.WriteLine(element);
            }
            // Performance on the test set
            Console.WriteLine("*** Performance on test set ***");
            Console.WriteLine("error: " + error_test);
            Console.WriteLine("r2s on test data");

            foreach (double element in r2_test)
            {
                Console.WriteLine(element);
            }


            //Console.WriteLine("r2 (alternative): " + alternative_r2_train);
            //Console.WriteLine("the loss function results on the training data were: " + error);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Error performing operation. " + e.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected ERROR: failed to perform multi var regression on the dataset");
            Console.WriteLine(ex.Message);
        }
    }
    */
}