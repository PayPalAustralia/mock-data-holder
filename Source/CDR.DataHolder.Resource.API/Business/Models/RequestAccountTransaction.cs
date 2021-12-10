using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CDR.DataHolder.Resource.API.Business.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CDR.DataHolder.Resource.API.Business.Models
{
    public class RequestAccountTransaction : IValidatableObject
    {
        [FromRoute(Name = "accountId")]
        public string AccountId { get; set; }

        [FromRoute(Name = "transactionId")]
        public string TransactionId { get; set; }

        public Guid CustomerId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            
            if (string.IsNullOrEmpty(this.AccountId))
                results.Add(new ValidationResult("Invalid account id.", new List<string> { "accountId" }));

            if (string.IsNullOrEmpty(this.TransactionId))
                results.Add(new ValidationResult("Invalid transaction id.", new List<string> { "transactionId" }));

            return results;
        }
    }
}
