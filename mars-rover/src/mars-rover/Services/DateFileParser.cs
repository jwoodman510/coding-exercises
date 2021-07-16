using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace mars_rover.Services
{
    public interface IDateFileParser
    {
        Task<IEnumerable<DateTime>> GetDatesAsync(string filePath);
    }

    public class DateFileParser : IDateFileParser
    {
        public async Task<IEnumerable<DateTime>> GetDatesAsync(string filePath)
        {
            var dates = new List<DateTime>();

            if (!File.Exists(filePath))
            {
                return dates;
            }

            using var filestream = File.OpenRead(filePath);
            using var streamReader = new StreamReader(filestream);

            var line = await streamReader.ReadLineAsync();

            while (line != null)
            {
                if (DateTime.TryParse(line, out DateTime dateTime))
                {
                    dates.Add(dateTime);
                }

                line = await streamReader.ReadLineAsync();
            }

            return dates.Distinct();
        }
    }
}
