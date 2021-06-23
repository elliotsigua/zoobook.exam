using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NodaTime;
using NodaTime.Text;

namespace Zoobook.Shared
{
    public class NodatimeUtility
    {
        /// <summary>
        /// Converts the datetime string to Nodatime Instant equivalent
        /// </summary>
        /// <param name="dateTimeString">DateTime string to be converted</param>
        public static Instant ConvertStringToInstant(string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString)) return new Instant();
            dateTimeString = dateTimeString.Contains("T") ? dateTimeString: $"{dateTimeString}T00:00:00Z";

            var pattern = InstantPattern.CreateWithInvariantCulture(Constants.DefaultDateTimeFormat);
            var parsedResult = pattern.Parse(dateTimeString);
            if (!parsedResult.Success)
            {
                throw new ArgumentException($"Invalid OffsetDateTimeString format '{dateTimeString}'.");
            }

            return parsedResult.GetValueOrThrow();
        }

        /// <summary>
        /// Converts the datetime to Nodatime Instant equivalent
        /// </summary>
        /// <param name="dateTime">DateTime to be converted</param>
        public static Instant ConvertDateTimeToInstant(DateTime dateTime)
        {
            var localDate = LocalDateTime.FromDateTime(dateTime);
            var zoned = localDate.InUtc();
            return zoned.ToInstant();
        }

        /// <summary>
        /// Converts string to Nodatime LocalTime equivalent
        /// </summary>
        /// <param name="timeString">Time string to be converted</param>
        public static LocalTime ConvertStringToLocalTime(string timeString)
        {
            if (string.IsNullOrWhiteSpace(timeString)) return new LocalTime();

            var pattern = LocalTimePattern.CreateWithInvariantCulture(Constants.DefaultTimeFormat);
            var parsedResult = pattern.Parse(timeString);
            if (!parsedResult.Success)
            {
                throw new ArgumentException($"Invalid Localtime format '{timeString}'.");
            }
            return parsedResult.GetValueOrThrow();
        }

        /// <summary>
        /// Get business days count between two dates
        /// </summary>
        /// <param name="dayFrom">Date from</param>
        /// <param name="dayTo">Date to</param>
        /// <param name="holidays">List of company holidates</param>
        public static decimal GetBusinessDays(Instant dayFrom, Instant dayTo, IEnumerable<Instant> holidays = null)
        {
            if (dayTo < dayFrom)
            {
                var holder = dayTo;
                dayTo = dayFrom;
                dayFrom = holder;
            }
            
            var dayFromLocalDate = dayFrom.InUtc();
            var dayToLocalDate = dayTo.InUtc();
            var dateDiff = dayTo - dayFrom;
            var fromDayOfWeek = (int)dayFromLocalDate.DayOfWeek;
            var toDayOfWeek = (int)dayToLocalDate.DayOfWeek;
            var difference = 0m;

            difference = (fromDayOfWeek <= toDayOfWeek) ?
                (((dateDiff.Days / Constants.DaysInWeek) * Constants.BusinessDaysInWeek) + Math.Max((Math.Min((toDayOfWeek + 1), 6) - fromDayOfWeek), 0))
                : (((dateDiff.Days / Constants.DaysInWeek) * Constants.BusinessDaysInWeek) + Math.Min((toDayOfWeek + 6) - Math.Min(fromDayOfWeek, 6), 5));

            var affectedHolidayCount = holidays?
                .Select(holiday => holiday.InUtc())
                .Count(holiday => dayFromLocalDate.LocalDateTime <= holiday.LocalDateTime
                    && holiday.LocalDateTime <= dayToLocalDate.LocalDateTime
                    && !(holiday.DayOfWeek == IsoDayOfWeek.Saturday || holiday.DayOfWeek == IsoDayOfWeek.Sunday)) ?? 0;
            difference -= affectedHolidayCount;

            return difference;
        }

        /// <summary>
        /// Get the intersected business days count on the two date ranges
        /// </summary>
        /// <param name="sourceFrom">Source Date From</param>
        /// <param name="sourceTo">Source Date To</param>
        /// <param name="destinationFrom">Destination Date From</param>
        /// <param name="destinationTo">Destination Date To</param>
        public static decimal GetDateRangeDifference(Instant sourceFrom, Instant sourceTo, Instant destinationFrom, Instant destinationTo)
        {
            return destinationFrom <= sourceFrom ?
                GetBusinessDays(sourceFrom, destinationTo)
                : GetBusinessDays(destinationFrom, sourceTo);
        }

        /// <summary>
        /// Retrieves the minimum and maximum dates from the list of entities
        /// </summary>
        /// <param name="entities">List of Object</param>
        /// <param name="propertyFrom">Date from property name of the object</param>
        /// <param name="propertyTo">Date trom property name of the object</param>
        public static InstantRange GetMinMaxFromEntities<TEntity>(List<TEntity> entities, string propertyFrom, string propertyTo)
        {
            var dateFrom = typeof(TEntity).GetProperties()
                .FirstOrDefault(propertyInfo => propertyInfo.Name.Equals(propertyFrom, StringComparison.OrdinalIgnoreCase)
                    && propertyInfo.PropertyType.Name == typeof(Instant).Name);
            var dateTo = typeof(TEntity).GetProperties()
                .FirstOrDefault(propertyInfo => propertyInfo.Name.Equals(propertyTo, StringComparison.OrdinalIgnoreCase)
                    && propertyInfo.PropertyType.Name == typeof(Instant).Name);
            if (dateFrom == null || dateTo == null)
                throw new MissingFieldException("DateFrom/DateTo fields were not found.");

            var instants = new List<Instant>();
            instants.AddRange(entities.Select(obj => (Instant)obj.GetType().GetProperty(dateFrom.Name).GetValue(obj)));
            instants.AddRange(entities.Select(obj => (Instant)obj.GetType().GetProperty(dateTo.Name).GetValue(obj)));

            return GetMinMaxFromInstants(instants);
        }

        /// <summary>
        /// Retrieve the minimum and maximum dates from the list of instants
        /// </summary>
        /// <param name="instants">List of Instants</param>
        public static InstantRange GetMinMaxFromInstants(List<Instant> instants)
        {
            if (instants.Count == 0) return null;

            return new InstantRange()
            {
                MinDate = instants.Min(),
                MaxDate = instants.Max()
            };
        }

        /// <summary>
        /// Retrieves the hours duration between two time
        /// </summary>
        /// <param name="timeIn">Time In</param>
        /// <param name="timeOut">Time Out</param>
        public static decimal GetHoursDuration(LocalTime timeIn, LocalTime timeOut)
        {
            var timeDiff = timeOut - timeIn;
            var hours = Math.Abs((decimal)timeDiff.Hours);
            var minutes = Math.Abs((decimal)timeDiff.Minutes);
            var seconds = Math.Abs((decimal)timeDiff.Seconds);

            var duration = Math.Max((hours + (minutes / Constants.MinutesPerHour) + (seconds / Constants.SecondsPerHour)), 0);
            if (timeOut < timeIn)
                duration = Constants.HoursPerDay - duration;

            return duration;
        }

        /// <summary>
        /// Returns the timestamp of the given instant
        /// </summary>
        /// <param name="instant">Instant where to extract the timestamp</param>
        public static string GetTimestamp(Instant instant)
        {
            return instant.ToDateTimeOffset().ToString("yyyyMMddHHmmssffff");
        }

        /// <summary>
        /// Get last week from latest date
        /// </summary>
        /// <returns></returns>
        public static Instant GetLastWeek()
        {
            var dateOffsetNow = new DateTimeOffset(
                DateTimeOffset.UtcNow.Year,
                DateTimeOffset.UtcNow.Month,
                DateTimeOffset.UtcNow.Day,
                0, 0, 0,
                new TimeSpan(0, 0, 0));

            return Instant.FromDateTimeOffset(dateOffsetNow)
                .Minus(Duration.FromDays(Constants.DaysInWeek));
        }

        /// <summary>
        /// Converts Instant to date string
        /// </summary>
        /// <param name="instant">Instant to convert</param>
        public static string ConvertToDateString(Instant instant)
        {
            return instant.ToString(Constants.DefaultDateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Get Dates in between two dates
        /// </summary>
        /// <param name="dateFrom">Date From</param>
        /// <param name="dateTo">Date To</param>
        /// <param name="includeWeekends">Include Weekends in List</param>
        public static IEnumerable<Instant> GetDatesInBetween(
            Instant dateFrom,
            Instant dateTo,
            bool includeWeekends = false)
        {
            var duration = dateTo.Minus(dateFrom).Days;
            var weekends = new[] { IsoDayOfWeek.Saturday, IsoDayOfWeek.Sunday };
            var betweenDates = Enumerable.Range(0, duration + 1)
                .Select(offset => dateFrom.Plus(Duration.FromDays(offset)));

            betweenDates = includeWeekends ?
                betweenDates : betweenDates.Where(date => !weekends.Contains(date.InUtc().DayOfWeek));

            return betweenDates;
        }

        /// <summary>
        /// Retrieves week range of provided date
        /// </summary>
        /// <param name="date">Date</param>
        public static InstantRange GetWeekRangeByDate(Instant date)
        {
            var weekRangesInMonth = GetWeeksInMonthByDate(date);
            var weekRange = weekRangesInMonth
                .FirstOrDefault(range => date >= range.MinDate.Value
                    && date <= range.MaxDate.Value);

            return weekRange;
        }

        /// <summary>
        /// Retrieves the weeks in a month by specified date
        /// </summary>
        /// <param name="startDate">Start date</param>
        public static List<InstantRange> GetWeeksInMonthByDate(Instant startDate)
        {
            var monthStartDate = Instant.FromDateTimeOffset(
                new DateTimeOffset(
                    startDate.InUtc().Year,
                    startDate.InUtc().Month,
                    1,
                    0, 0, 0, new TimeSpan(0, 0, 0)));
            var monthEndDate = Instant.FromDateTimeOffset(
                new DateTimeOffset(
                    startDate.InUtc().Year,
                    startDate.InUtc().Month + 1,
                    1,
                    0, 0, 0, new TimeSpan(0, 0, 0)))
                .Minus(Duration.FromDays(1));

            var utcDate = startDate.InUtc();
            var daysInMonth = utcDate.Calendar.GetDaysInMonth(utcDate.Year, utcDate.Month);
            var weekRanges = new List<InstantRange>();

            var weekRange = new InstantRange();
            var weekStartDate = monthStartDate;
            for (var dayIndex = 0; dayIndex < daysInMonth; ++dayIndex)
            {
                var date = weekStartDate.Plus(Duration.FromDays(dayIndex));
                if (date == monthEndDate || date.InUtc().DayOfWeek == IsoDayOfWeek.Sunday)
                {
                    weekRange.MinDate ??= date;
                    weekRange.MaxDate = date;
                    weekRanges.Add(weekRange);
                    weekRange = new InstantRange();
                }
                else if (!weekRange.MinDate.HasValue)
                {
                    weekRange.MinDate = date;
                }
            }

            var firstWeek = weekRanges.FirstOrDefault();
            if (firstWeek.MaxDate.Value.Minus(firstWeek.MinDate.Value).Days < 3)
            {
                weekRanges = weekRanges
                    .Where(range => range != firstWeek)
                    .ToList();
                weekRanges.FirstOrDefault().MinDate = firstWeek.MinDate;
            }

            var lastWeek = weekRanges.LastOrDefault();
            if (lastWeek.MaxDate.Value.Minus(lastWeek.MinDate.Value).Days < 3)
            {
                weekRanges = weekRanges
                    .Where(range => range != lastWeek)
                    .ToList();
                weekRanges.LastOrDefault().MaxDate = lastWeek.MaxDate;
            }

            return weekRanges;
        }
    }
}
