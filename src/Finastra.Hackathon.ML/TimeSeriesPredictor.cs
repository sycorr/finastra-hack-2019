using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace Finastra.Hackathon.ML
{
    public class TimeSeriesPredictor
    {
        public IEnumerable<RatioAnalysis> GetValues()
        {
            var data = GetData();
            var mlContext = new MLContext(seed: 1);

            IEstimator<ITransformer> costofgoodsForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RationAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.CostOfGoods), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RationAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RationAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            IEstimator<ITransformer> inventoryForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RationAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.Inventory), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RationAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RationAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            IEstimator<ITransformer> turnoverForcaster = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(RationAnalysisPrediction.Forecasted),
                inputColumnName: nameof(RatioAnalysis.Turnover), // This is the column being forecasted.
                windowSize: 12, // Window size is set to the time period represented in the product data cycle; our product cycle is based on 12 months, so this is set to a factor of 12, e.g. 3.
                seriesLength: data.Count(), // This parameter specifies the number of data points that are used when performing a forecast.
                trainSize: data.Count(), // This parameter specifies the total number of data points in the input time series, starting from the beginning.
                horizon: 3, // Indicates the number of values to forecast; 3 indicates that the next 3 months of product units will be forecasted.
                confidenceLevel: 0.75f, // Indicates the likelihood the real observed value will fall within the specified interval bounds.
                confidenceLowerBoundColumn: nameof(RationAnalysisPrediction.ConfidenceLowerBound), //This is the name of the column that will be used to store the lower interval bound for each forecasted value.
                confidenceUpperBoundColumn: nameof(RationAnalysisPrediction.ConfidenceUpperBound)); //This is the name of the column that will be used to store the upper interval bound for each forecasted value.

            // Fit the forecasting model to the specified product's data series.
            ITransformer costofgoodsTransformer = costofgoodsForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));
            ITransformer inventoryTransformer = inventoryForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));
            ITransformer turnoverTransformer = turnoverForcaster.Fit(mlContext.Data.LoadFromEnumerable(data));

            // Create the forecast engine used for creating predictions.
            TimeSeriesPredictionEngine<RatioAnalysis, RationAnalysisPrediction> inventoryEngine = inventoryTransformer.CreateTimeSeriesEngine<RatioAnalysis, RationAnalysisPrediction>(mlContext);
            TimeSeriesPredictionEngine<RatioAnalysis, RationAnalysisPrediction> turneroverEngine = turnoverTransformer.CreateTimeSeriesEngine<RatioAnalysis, RationAnalysisPrediction>(mlContext);
            TimeSeriesPredictionEngine<RatioAnalysis, RationAnalysisPrediction> costofgoodEngine = costofgoodsTransformer.CreateTimeSeriesEngine<RatioAnalysis, RationAnalysisPrediction>(mlContext);

            // Get the prediction; this will include the forecasted turnover for the next 3 months since this 
            //the time period specified in the `horizon` parameter when the forecast estimator was originally created.
            var turnoverPrediction = turneroverEngine.Predict();
            var costPrediction = costofgoodEngine.Predict();
            var inventoryPrediction = inventoryEngine.Predict();

            var last = data.Last();

            for(int i = 0; i < turnoverPrediction.Forecasted.Count(); i++)
            {
                data.Add(new RatioAnalysis
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

            return data;
        }

        public List<RatioAnalysis> GetData()
        {
            var data =
                "Date,Cost of Goods,Inventory,Turn Over,Turn Days\r\n7/1/2016,\"75,000\",\"20,000\",3.75,96.00\r\n8/1/2016,\"75,000\",\"24,000\",3.13,115.20\r\n9/1/2016,\"75,000\",\"13,400\",5.60,64.32\r\n10/1/2016,\"75,000\",\"7,500\",10.00,36.00\r\n11/1/2016,\"75,000\",\"7,150\",10.49,34.32\r\n12/1/2016,\"75,000\",\"7,050\",10.64,33.84\r\n1/1/2017,\"75,000\",\"12,500\",6.00,60.00\r\n2/1/2017,\"75,000\",\"21,000\",3.57,100.80\r\n3/1/2017,\"75,000\",\"19,700\",3.81,94.56\r\n4/1/2017,\"75,000\",\"22,300\",3.36,107.04\r\n5/1/2017,\"75,000\",\"18,900\",3.97,90.72\r\n6/1/2017,\"75,000\",\"18,000\",4.17,86.40\r\n7/1/2017,\"75,000\",\"19,800\",3.79,95.04\r\n8/1/2017,\"75,000\",\"22,900\",3.28,109.92\r\n9/1/2017,\"75,000\",\"13,000\",5.77,62.40\r\n10/1/2017,\"75,000\",\"7,000\",10.71,33.60\r\n11/1/2017,\"75,000\",\"7,200\",10.42,34.56\r\n12/1/2017,\"75,000\",\"6,950\",10.79,33.36\r\n1/1/2018,\"75,000\",\"11,900\",6.30,57.12\r\n2/1/2018,\"75,000\",\"20,500\",3.66,98.40\r\n3/1/2018,\"75,000\",\"19,200\",3.91,92.16\r\n4/1/2018,\"75,000\",\"23,550\",3.18,113.04\r\n5/1/2018,\"75,000\",\"18,100\",4.14,86.88\r\n6/1/2018,\"75,000\",\"18,900\",3.97,90.72\r\n7/1/2018,\"75,000\",\"19,000\",3.95,91.20\r\n8/1/2018,\"75,000\",\"26,000\",2.88,124.80\r\n9/1/2018,\"75,000\",\"15,600\",4.81,74.88\r\n10/1/2018,\"75,000\",\"8,100\",9.26,38.88\r\n11/1/2018,\"75,000\",\"7,650\",9.80,36.72\r\n12/1/2018,\"75,000\",\"6,850\",10.95,32.88\r\n1/1/2019,\"75,000\",\"10,200\",7.35,48.96\r\n2/1/2019,\"75,000\",\"24,500\",3.06,117.60\r\n3/1/2019,\"75,000\",\"18,750\",4.00,90.00\r\n4/1/2019,\"75,000\",\"21,900\",3.42,105.12\r\n5/1/2019,\"75,000\",\"17,500\",4.29,84.00\r\n6/1/2019,\"75,000\",\"21,200\",3.54,101.76\r\n7/1/2019,\"75,000\",\"17,400\",4.31,83.52\r\n8/1/2019,\"75,000\",\"24,000\",3.13,115.20\r\n9/1/2019,\"75,000\",\"13,400\",5.60,64.32\r\n";

            return new FileHelperEngine<RatioAnalysis>().ReadStringAsList(data).ToList();
        }
    }
}
 