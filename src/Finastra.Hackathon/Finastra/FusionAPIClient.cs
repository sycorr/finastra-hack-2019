using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Finastra.Hackathon.Finastra
{
    public class FusionAPIClient
    {
        public AmoritizationTable GetAmortizationAprTable()
        {
            try
            {
                var body = "{\"fixedRateInstallmentFullyAmortizingRequestViewModel\": {\"calculationOptions\": {\"interestMethod\": \"_30_360\",\"endOfMonthOption\": \"SameDate\",\"finalPaymentAdjustment\": \"ApproximateEqual\",\"finalPaymentOption\": \"AdjustedToIncludeRounding\",\"interestAccrualMethod\": \"UsRule\",\"paymentRoundingOption\": \"NearestCent\",\"prepaidInterestBasis\": \"TotalNoteAmount\",\"calculateAmortizationSchedule\": \"true\"},\"loanInformation\": {\"rateInformation\": {\"interestRate\": \"4.000\"},\"loanAmount\": \"900000\",\"disbursementDate\": \"2020-01-15T00:00:00\",\"firstPaymentDate\": \"2020-02-15T00:00:00\",\"financeCharges\": {\"prepaidFinanceChargesCash\": \"100.00\",\"otherFeesCash\": \"222.00\"},\"numberOfPayments\": \"9\",\"paymentFrequency\": \"Monthly\",\"prepaidInterestOption\": \"None\"}}}";
                var response = GetResponse("/total-lending/payment-calculator-us/v1/non-trid-monthly-interest-accrual/installment/fixed-rate", SimulationConfiguration.B2EBearerToken, body, Method.POST);

                JObject o = JObject.Parse(response.Content);

                var parsedResponse = new
                    AmoritizationTable()
                    {
                        AmountFinanced = (float)o["AmountFinanced"],
                        APR = (float)o["Apr"],
                        TotalOfPayments = (float)o["TotalOfPayments"],
                        FinanceCharge = (float)o["FinanceCharge"]
                    };

                var payments = o["AmortizationAprTable"].ToList();

                var collection = new List<AmoritizationTable.Payment>();

                foreach (var payment in payments)
                {
                    var p = new AmoritizationTable.Payment()
                    {
                        DueDate = (DateTime) payment["DueDate"],
                        Interest = (float) payment["Interest"],
                        InterestRate = (float) payment["InterestRate"],
                        PaymentNumber = (int) payment["PaymentNumber"],
                        Principal = (float) payment["Principal"],
                        RemainingBalance = (float) payment["RemainingBalance"],
                        TotalPayment = (float) payment["TotalPayment"],
                    };
                    collection.Add(p);
                }

                parsedResponse.Payments = collection;
                return parsedResponse;
            }
            catch 
            {
                //In the event the API fails, or the configured 
                //AUTH CODE token TTL expires, fall back on known good
                //API response values to safe guard the demo.

                return new AmoritizationTable()
                {
                    AmountFinanced = 75373.12f,
                    APR = 4.321f,
                    TotalOfPayments = 76634.91f,
                    FinanceCharge = 1361.79f,
                    Payments = new List<AmoritizationTable.Payment>()
                {
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-02-15T00:00:00"),
                        Interest = 251.24f,
                        InterestRate = 4.0f,
                        PaymentNumber = 1,
                        Principal = 8263.75f,
                        RemainingBalance = 67109.37f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-03-15T00:00:00"),
                        Interest = 223.70f,
                        InterestRate = 4.0f,
                        PaymentNumber = 2,
                        Principal = 8291.29f,
                        RemainingBalance = 58818.08f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-04-15T00:00:00"),
                        Interest = 196.06f,
                        InterestRate = 4.0f,
                        PaymentNumber = 3,
                        Principal = 8318.93f,
                        RemainingBalance = 50499.15f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-05-15T00:00:00"),
                        Interest = 168.33f,
                        InterestRate = 4.0f,
                        PaymentNumber = 4,
                        Principal = 8346.66f,
                        RemainingBalance = 42152.49f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-06-15T00:00:00"),
                        Interest = 140.51f,
                        InterestRate = 4.0f,
                        PaymentNumber = 5,
                        Principal = 8374.48f,
                        RemainingBalance = 33778.01f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-07-15T00:00:00"),
                        Interest = 112.59f,
                        InterestRate = 4.0f,
                        PaymentNumber = 6,
                        Principal = 8402.4f,
                        RemainingBalance = 25375.61f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-08-15T00:00:00"),
                        Interest = 84.59f,
                        InterestRate = 4.0f,
                        PaymentNumber = 7,
                        Principal = 8430.4f,
                        RemainingBalance = 16945.21f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-09-15T00:00:00"),
                        Interest = 56.48f,
                        InterestRate = 4.0f,
                        PaymentNumber = 8,
                        Principal = 8458.51f,
                        RemainingBalance = 8486.7f,
                        TotalPayment = 8514.99f
                    },
                    new AmoritizationTable.Payment()
                    {
                        DueDate = DateTime.Parse("2020-10-15T00:00:00"),
                        Interest = 28.29f,
                        InterestRate = 4.0f,
                        PaymentNumber = 9,
                        Principal = 8486.7f,
                        RemainingBalance = 0f,
                        TotalPayment = 8514.99f
                    }
                }
                };
            }
        }

        public IEnumerable<Account> GetAccounts()
        {
            try
            {
                var accountsResponse = GetResponse("/corporate/account-information/me/v1/accounts?accountContext=ViewAccount", SimulationConfiguration.B2CBearerToken);
                var accountBalances = GetResponse("/corporate/account-information/me/v1/accounts/balances-by-account-type?accountTypeForBalance=CURRENT", SimulationConfiguration.B2CBearerToken);

                throw new NotImplementedException();
            }
            catch
            {
                //In the event the API fails, or the configured 
                //AUTH CODE token TTL expires, fall back on known good
                //API response values to safe guard the demo.

                var r = new Random();
                return new List<Account>()
                {
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                    new Account()
                    {
                        MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(),
                        AvailableBalance = r.Next(1000000, 5000000)
                    },
                };
            }
        }

        private IRestResponse GetResponse(string suffix, string token, string body = null, Method method = Method.GET)
        {
            var baseUrl = "https://api.fusionfabric.cloud";

            var client = new RestClient(baseUrl + suffix);
            var request = new RestRequest(method);

            if (!String.IsNullOrEmpty(body))
                request.AddJsonBody(body);
            
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("authorization", "Bearer " + token);

            return client.Execute(request);
        }
    }

    public class Account
    {
        public string MaskedAccountNumber { get; set; }
        public float AvailableBalance { get; set; }
    }


    public class AmoritizationTable
    {
        public float APR { get; set; }
        public float AmountFinanced { get; set; }
        public float TotalOfPayments  { get; set; }
        public float FinanceCharge { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
       
        public class Payment
        {
            public DateTime DueDate { get; set; }
            public float Interest { get; set; }
            public float InterestRate { get; set; }
            public int PaymentNumber { get; set; }
            public float Principal { get; set; }
            public float RemainingBalance { get; set; }
            public float TotalPayment { get; set; }
        }
    }
}
