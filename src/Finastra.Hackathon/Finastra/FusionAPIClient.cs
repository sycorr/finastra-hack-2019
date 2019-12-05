using System;
using System.Collections.Generic;
using System.Text;

namespace Finastra.Hackathon.Finastra
{
    public class FusionAPIClient
    {
        //Fallback JSON payloads in case API fails mid-demo
        private string _getAccountsByAccountType = "";
        private string _balancesByAccountType = "";
        private string _nontridMonthlyInterestAccrual = "";

        private string _nontridPostData = "";

        public AmoritizationTable GetAmortizationAprTable()
        {
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
                    }
                }
            };
        }

        public IEnumerable<Account> GetAccounts()
        {
            var r = new Random();
            return new List<Account>()
            {
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
                new Account() { MaskedAccountNumber = "xxxx-xxxx-xxxx-xxx-xxxx-" + r.Next(10, 99).ToString(), AvailableBalance = r.Next(1000000, 5000000)},
            };
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


//{
//"fixedRateInstallmentFullyAmortizingRequestViewModel": {
//    "calculationOptions": {
//        "interestMethod": "_30_360",
//        "endOfMonthOption": "SameDate",
//        "finalPaymentAdjustment": "ApproximateEqual",
//        "finalPaymentOption": "AdjustedToIncludeRounding",
//        "interestAccrualMethod": "UsRule",
//        "paymentRoundingOption": "NearestCent",
//        "prepaidInterestBasis": "TotalNoteAmount",
//        "calculateAmortizationSchedule": "true"
//    },
//    "loanInformation": {
//        "rateInformation": {
//            "interestRate": "4.000"
//        },
//        "loanAmount": "100000.00",
//        "disbursementDate": "2019-04-15T00:00:00",
//        "firstPaymentDate": "2019-05-15T00:00:00",
//        "financeCharges": {
//            "prepaidFinanceChargesCash": "100.00",
//            "otherFeesCash": "222.00"
//        },
//        "numberOfPayments": "4",
//        "paymentFrequency": "Monthly",
//        "prepaidInterestOption": "None"
//    }
//}
//}


//{
//  "Errors": [],
//  "Apr": 4.321,
//  "TotalOfPayments": 76634.91,
//  "RegularPaymentAmount": 8514.99,
//  "FinalPaymentAmount": 8514.99,
//  "AmountFinanced": 75273.12,
//  "FinanceCharge": 1361.79,
//  "PrepaidInterestTotal": 0.0,
//  "PrepaidInterestDaily": 0.0,
//  "NumberOfDaysOfPrepaidInterest": 0,
//  "InitialTridPayment": 8514.99,
//  "AmortizationAprTable": [
//    {
//      "DueDate": "2020-02-15T00:00:00",
//      "Interest": 251.24,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 1,
//      "Principal": 8263.75,
//      "RemainingBalance": 67109.37,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-03-15T00:00:00",
//      "Interest": 223.7,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 2,
//      "Principal": 8291.29,
//      "RemainingBalance": 58818.08,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-04-15T00:00:00",
//      "Interest": 196.06,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 3,
//      "Principal": 8318.93,
//      "RemainingBalance": 50499.15,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-05-15T00:00:00",
//      "Interest": 168.33,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 4,
//      "Principal": 8346.66,
//      "RemainingBalance": 42152.49,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-06-15T00:00:00",
//      "Interest": 140.51,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 5,
//      "Principal": 8374.48,
//      "RemainingBalance": 33778.01,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-07-15T00:00:00",
//      "Interest": 112.59,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 6,
//      "Principal": 8402.4,
//      "RemainingBalance": 25375.61,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-08-15T00:00:00",
//      "Interest": 84.59,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 7,
//      "Principal": 8430.4,
//      "RemainingBalance": 16945.21,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-09-15T00:00:00",
//      "Interest": 56.48,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 8,
//      "Principal": 8458.51,
//      "RemainingBalance": 8486.7,
//      "TotalPayment": 8514.99
//    },
//    {
//      "DueDate": "2020-10-15T00:00:00",
//      "Interest": 28.29,
//      "InterestRate": 4.0,
//      "MortgageInsurance": 0.0,
//      "PaymentNumber": 9,
//      "Principal": 8486.7,
//      "RemainingBalance": 0.0,
//      "TotalPayment": 8514.99
//    }
//  ]
//}