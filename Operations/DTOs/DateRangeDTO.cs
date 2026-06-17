using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Operations.DTOs
{
    public class DateRangeDTO
    {
        public DateTime Desde { get; set; } = new DateTime(2026, 1, 1);
        public DateTime Hasta { get; set; } = new DateTime(2026, 3, 31);
    }

    public class NumericDateRangeDTO
    {
        public int Desde { get; set; }
        public int Hasta { get; set; }
    }

    public static class DateRangeDTOParser
    {
        public static NumericDateRangeDTO ToNumericDateRangeDTO(this DateRangeDTO dateRangeDTO)
        {
            return new NumericDateRangeDTO
            {
              Desde = int.Parse(dateRangeDTO.Desde.ToString("yyyyMMdd")),
              Hasta  = int.Parse(dateRangeDTO.Hasta.ToString("yyyyMMdd"))
            };
        }
    }
}