using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace Finastra.Hackathon.ML
{
    public class FastTreePredictor
    {
        public IEnumerable<RatioAnalysis> GetValues()
        {
            var data = GetData();
            var mlContext = new MLContext(seed: 1);
            var trainingDataView = mlContext.Data.LoadFromEnumerable(data);

            var trainer = mlContext.Regression.Trainers.FastTreeTweedie(labelColumnName: "Label", featureColumnName: "Features");

            var trainingPipeline = mlContext.Transforms.Concatenate(outputColumnName: "NumFeatures", nameof(RatioAnalysis.Date), nameof(RatioAnalysis.Turnover), nameof(RatioAnalysis.TurnDays))
                //.Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "CatFeatures", inputColumnName: nameof(ProductData.productId)))
                //.Append(mlContext.Transforms.Concatenate(outputColumnName: "Features", "NumFeatures", "CatFeatures"))
                .Append(mlContext.Transforms.Concatenate(outputColumnName: "Features", "NumFeatures"))
                //.Append(mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(ProductData.next)))
                .Append(trainer);

            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            Console.WriteLine("=============== Cross-validating to get Regression model's accuracy metrics ===============");
            var crossValidationResults = mlContext.Regression.CrossValidate(data: trainingDataView, estimator: trainingPipeline, numberOfFolds: 6, labelColumnName: "Label");
            
            //ConsoleHelper.PrintRegressionFoldsAverageMetrics(trainer.ToString(), crossValidationResults);

            // Train the model.
            var model = trainingPipeline.Fit(trainingDataView);

            var forecastEngine = mlContext.Model.CreatePredictionEngine<RatioAnalysis, RationAnalysisPrediction>(model);

            // Get the prediction; this will include the forecasted turnover for the next 3 months since this 
            //the time period specified in the `horizon` parameter when the forecast estimator was originally created.
            var turnoverPredictions = new List<RationAnalysisPrediction>();

            //var last = data.Last();

            //for (int i = 0; i < turnoverPredictions.Count(); i++)
            //{
            //    data.Add(new RatioAnalysis
            //    {
            //        Date = last.Date.AddMonths(i + 1),
            //        InventoryTurnover = turnoverPredictions[i],
            //        PredictionDelta = turnoverPrediction.ConfidenceUpperBound[i] - turnoverPrediction.ForecastedInventoryTurnover[i]
            //    });
            //}

            return data;
        }

        public List<RatioAnalysis> GetData()
        {
            return new List<RatioAnalysis>()
            {
                new RatioAnalysis { Date = new DateTime(2017, 1, 1), Turnover = 1.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 2, 1), Turnover = 1.5f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 3, 1), Turnover = 2.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 4, 1), Turnover = 2.9f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 5, 1), Turnover = 6.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 6, 1), Turnover = 15.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 7, 1), Turnover = 22.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 8, 1), Turnover = 25.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 9, 1), Turnover = 0.5f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 10, 1), Turnover = 0.4f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 11, 1), Turnover = 0.05f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2017, 12, 1), Turnover = 0.2f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 1, 1), Turnover = 1.05f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 2, 1), Turnover = 1.72f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 3, 1), Turnover = 3.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 4, 1), Turnover = 3.1f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 5, 1), Turnover = 5.8f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 6, 1), Turnover = 14.4f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 7, 1), Turnover = 20.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 8, 1), Turnover = 22.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 9, 1), Turnover = 5.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 10, 1), Turnover = 0.3f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 11, 1), Turnover = 0.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2018, 12, 1), Turnover = 0.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 1, 1), Turnover = 0.8f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 2, 1), Turnover = 1.5f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 3, 1), Turnover = 2.1f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 4, 1), Turnover = 3.8f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 5, 1), Turnover = 7.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 6, 1), Turnover = 16.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 7, 1), Turnover = 23.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 8, 1), Turnover = 29.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 9, 1), Turnover = 6.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 10, 1), Turnover = 0.2f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 11, 1), Turnover = 0.0f, TurnDays = 0.0f },
                new RatioAnalysis { Date = new DateTime(2019, 12, 1), Turnover = 0.2f, TurnDays = 0.0f },
            };
        }
    }
}
