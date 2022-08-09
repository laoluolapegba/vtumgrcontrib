using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sales_Mgmt.Services.Simplex.Api.Helpers.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidatableObject : IValidatableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
