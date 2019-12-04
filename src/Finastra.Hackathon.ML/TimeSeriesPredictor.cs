using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;
using Finastra.Hackathon.Accounting;

namespace Finastra.Hackathon.ML
{
    public class TimeSeriesPredictor
    {
        public IEnumerable<RatioAnalysis> Predict(IEnumerable<RatioAnalysis> data)
        {
            var mlContext = new MLContext(seed: 1);

            IEstimator<ITransformer> costofgoodsForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RatioAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.CostOfGoods), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            IEstimator<ITransformer> inventoryForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RatioAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.Inventory), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            IEstimator<ITransformer> turnoverForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RatioAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.Turnover), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RatioAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            // Fit the forecasting model to the specified product's data series.
            ITransformer costofgoodsTransformer = costofgoodsForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));
            ITransformer inventoryTransformer = inventoryForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));
            ITransformer turnoverTransformer = turnoverForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));

            // Create the forecast engine used for creating predictions.
            TimeSeriesPredictionEngine<RatioAnalysis, RatioAnalysisPrediction> inventoryEngine = inventoryTransformer.CreateTimeSeriesEngine<RatioAnalysis, RatioAnalysisPrediction>(mlContext);
            TimeSeriesPredictionEngine<RatioAnalysis, RatioAnalysisPrediction> turneroverEngine = turnoverTransformer.CreateTimeSeriesEngine<RatioAnalysis, RatioAnalysisPrediction>(mlContext);
            TimeSeriesPredictionEngine<RatioAnalysis, RatioAnalysisPrediction> costofgoodEngine = costofgoodsTransformer.CreateTimeSeriesEngine<RatioAnalysis, RatioAnalysisPrediction>(mlContext);

            // Get the prediction; this will include the forecasted turnover for the next 3 months since this 
            //the time period specified in the `horizon` parameter when the forecast estimator was originally created.
            var turnoverPrediction = turneroverEngine.Predict();
            var costPrediction = costofgoodEngine.Predict();
            var inventoryPrediction = inventoryEngine.Predict();

            var last = data.Last();
            var retVal = data.ToList();

            for(int i = 0; i < turnoverPrediction.Forecasted.Count(); i++)
            {
                retVal.Add(new RatioAnalysis
                {
                    Date = last.Date.AddMonths(i + 1),
                    CostOfGoods = costPrediction.Forecasted[i],
                    CostOfGoodsDelta = costPrediction.ConfidenceUpperBound[i] - costPrediction.Forecasted[i],
                    Inventory = inventoryPrediction.Forecasted[i],
                    InventoryDelta = inventoryPrediction.ConfidenceUpperBound[i] - inventoryPrediction.Forecasted[i],
                    Turnover = turnoverPrediction.Forecasted[i],
                    TurnoverDelta = turnoverPrediction.ConfidenceUpperBound[i] - turnoverPrediction.Forecasted[i]
                });
            }

            return retVal;
        }
    }
}
 