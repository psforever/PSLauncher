using PSF.Data.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSF.Data.Extensions
{
    public static class SecurePropertyExtension
    {
        /// <summary>
        /// Retrieves value of 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetSecureValue(this string obj)
        {
            var t = obj.GetType();
            
            if (t.IsDefined(typeof(SecureProperty), true))
            {
                return SecureProperty.GetValueFromSecureProperty($"{t.FullName}::{t.Name}");
            }

            return null;
        }
    }
}
