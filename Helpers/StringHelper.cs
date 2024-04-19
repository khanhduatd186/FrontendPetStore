using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Helpers;

public static class StringHelper
{
    public static string ParseGuidToStr(this string value)
    {
        var v = Guid.TryParse(value, out _);
        return v ? value : Guid.Empty.ToString();
    }
    
    public static Guid ParseGuidToGuid(this string value)
    {
        var v = Guid.TryParse(value, out var guid);
        return v ? guid : Guid.Empty;
    }
    
    public static string ConvertBase64String(this string value)
    {
        try
        {
            byte[] data = Convert.FromBase64String(value);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
        catch (Exception e)
        {
            return "";
        }
    }

    public static string RemoveSpaces(this string value)
    {
        return Regex.Replace(value, @"\s+", "");
    }

    public static string UpperCaseFirstCharacters(string input)
    {
        return input.Length switch
        {
            0 => "",
            1 => char.ToUpper(input[0]).ToString(),
            _ => char.ToUpper(input[0]) + input.Substring(1)
        };
    }

    public static string DiscordHyperlink(string title, string link)
    {
        return $"[{title}]({link})";
    }

    public static string ToFileSize(long bytes)
    {
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number = number / 1024;
            counter++;
        }

        return $"{number:n1} {Suffixes[counter]}";
    }

    public static string BackupFileName(string databaseName)
    {
        return $"{databaseName} - {VietNameseDateTimeNowToString()} - Backup.sql";
    }

    public static string ImageFileName(IFormFile file, string userName)
    {
        string stringName;
        var ticks = DateTime.Now.Ticks.ToString();

        if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".PNG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".png";
        }
        else if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".JPG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }
        else if (file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".JPEG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }
        else if (file.FileName.EndsWith(".gif") || file.FileName.EndsWith(".GIF"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".gif";
        }
        else if (file.FileName.EndsWith(".webp") || file.FileName.EndsWith(".WEBP"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".webp";
        }
        else
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }

        return $"{stringName}";
    }

    public static string ImageFileName(IFormFile file)
    {
        string userName = Guid.NewGuid().ToString().Split("-").First();
        string stringName;
        var ticks = DateTime.Now.Ticks.ToString();

        if (file.FileName.EndsWith(".png") || file.FileName.EndsWith(".PNG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".png";
        }
        else if (file.FileName.EndsWith(".jpg") || file.FileName.EndsWith(".JPG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }
        else if (file.FileName.EndsWith(".jpeg") || file.FileName.EndsWith(".JPEG"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }
        else if (file.FileName.EndsWith(".gif") || file.FileName.EndsWith(".GIF"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".gif";
        }
        else if (file.FileName.EndsWith(".webp") || file.FileName.EndsWith(".WEBP"))
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".webp";
        }
        else
        {
            stringName = userName + "_" + ticks + "_" + UniqueString() + ".jpg";
        }

        return $"{stringName}";
    }

    public static string UniqueString()
    {
        return Guid.NewGuid().ToString().Split("-")[0];
    }

    public static bool ContainsAnyCaseInvariant(string str, char character)
    {
        return str.IndexOf(character, StringComparison.OrdinalIgnoreCase) != -1;
    }

    public static bool ContainsAnyCase(string str, char character)
    {
        return str.IndexOf(character, StringComparison.CurrentCultureIgnoreCase) != -1;
    }

    public static string GitMessageCommit(string userName)
    {
        return $"{userName} {VietNameseDateTimeNowToString()}";
    }

    public static string VietNameseDateTimeNowToStringForFileName()
    {
        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneVietnamese());
        var nameDate = currentTime.ToString(CultureInfo.InvariantCulture)
            .Replace("/", "-")
            .Replace(":", ".");
        nameDate = nameDate.Replace(" ", "_");
        return nameDate;
    }

    public static string ToExcelFileNameVN(this string heading)
    {
        var currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneVietnamese()).ToString("dd/MM/yyy");
        var nameDate = currentTime.ToString(CultureInfo.InvariantCulture)
            .Replace("/", "-")
            .Replace(":", "@");
        nameDate = nameDate.Replace(" ", "_");
        var nameDateStr = nameDate.Split("@").First();
        return $"{heading}_{nameDateStr}_{Guid.NewGuid().ToString().Split("-").First().ToUpper()}.xlsx";
    }

    public static string VietNameseDateTimeNowToString()
    {
        DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneVietnamese());
        string nameDate = currentTime.ToString(CultureInfo.InvariantCulture);
        return nameDate;
    }

    public static DateTime VietNameseDateTimeNow()
    {
        DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneVietnamese());
        return currentTime;
    }

    public static string TruncateHtml(this string content, int lengthWords, int lengthString, bool addEllipsis)
    {
        if (content.Length <= lengthWords)
        {
            return content;
        }

        string s2 = "";
        string[] strSplit = content.Split(' ');

        for (int i = 0; i < strSplit.Length; i++)
        {
            s2 += strSplit[i] + " ";
            if (i == lengthWords)
            {
                break;
            }
        }

        if (CountWords(content) < lengthWords)
        {
            for (int i = 0; i < lengthString - strSplit.Length; i++)
            {
                s2 += "&nbsp; ";
            }
        }

        if (addEllipsis)
        {
            s2 += "...";
        }

        return s2;
    }

    public static string RandomString()
    {
        char[] stringChars = new char[3];
        Random random = new Random();

        for (int i = 0; i < 1; i++)
        {
            stringChars[i] = NumberCharacters[random.Next(NumberCharacters.Length)];
        }

        for (int i = 1; i < 2; i++)
        {
            stringChars[i] = LowerCaseCharacters[random.Next(LowerCaseCharacters.Length)];
        }

        for (int i = 2; i < 3; i++)
        {
            stringChars[i] = UpperCaseCharacters[random.Next(UpperCaseCharacters.Length)];
        }

        return new string(stringChars);
    }

    public static string Truncate(this string? value, int maxLength, string truncationSuffix = "...")
    {
        if (IsNullOrEmptyOrWhiteSpace(value))
        {
            return "";
        }

        return value.Length > maxLength
            ? value.Substring(0, maxLength) + truncationSuffix
            : value;
    }

    public static string VietnameseToNormalString(this string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
            {
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
        }

        return str;
    }

    public static string ToStringCode(this string str)
    {
        str = SpecialCharacters.Aggregate(str, (current, item) => current.Replace(item, string.Empty));
        return VietnameseToNormalString(str).ToLower().Replace(' ', '-');
    }

    public static string UniqueUrl(this string title)
    {
        string guid = Guid.NewGuid().ToString().Split("-")[1];
        string url = ToStringCode(title);
        return $"{url}-{guid}{RandomString()}";
    }

    public static string UniqueUrlWithBaseCode(this string title, string code)
    {
        string url = ToStringCode(title);
        return $"{url}-{code}";
    }

    private static int CountWords(string content)
    {
        if (IsNullOrEmptyOrWhiteSpace(content) || string.IsNullOrWhiteSpace(content))
        {
            return 0;
        }

        string[] strSplit = content.Split(' ');

        return strSplit.Length;
    }
    
    public static string FormatMoney(double total)
    {
        string str = $"{total:C}".Replace("$", string.Empty) + " VND";
        return str.Replace("¤", string.Empty);
    }

    public static string FormatDoubleCurrency(this double amount)
    {
        return string.Format("{0:n0}", amount);
    }

    public static string FormatIntCurrency(this int amount)
    {
        return string.Format("{0:n0}", amount);
    }

    public static string ToShortenMoney(int? minPrice, int? maxPrice)
    {
        const string baseString = "triệu/tháng";

        if (minPrice == null && maxPrice == null)
        {
            return "Dãy trọ hiện chưa có phòng";
        }

        if (minPrice <= 0 && maxPrice <= 0)
        {
            return "Dãy trọ hiện chưa có phòng";
        }

        string toStringMinPrice = string.Format("{0:#,##0.##}", minPrice);
        string toStringMaxPrice = string.Format("{0:#,##0.##}", maxPrice);

        string strMin = "";
        string strMax = "";

        if (minPrice >= 100_000 && minPrice < 1_000_000)
        {
            strMin = toStringMinPrice.Split(",")[0] + "k";
        }
        else if (minPrice >= 1_000_000 && minPrice < 10_000_000)
        {
            strMin = toStringMinPrice.Split(",")[0];
            int minPriceParse = Convert.ToInt32(toStringMinPrice.Split(",")[1]);
            if (minPriceParse >= 100)
            {
                strMin = strMin + "." + minPriceParse.ToString().Substring(0, minPriceParse.ToString().Length - 2);
            }
        }
        else if (minPrice >= 10_000_000 && minPrice < 100_000_000)
        {
            strMin = toStringMinPrice.Split(",")[0];
            int minPriceParse = Convert.ToInt32(toStringMinPrice.Split(",")[1]);
            if (minPriceParse >= 100)
            {
                strMin = strMin + "." + minPriceParse.ToString().Substring(0, minPriceParse.ToString().Length - 2);
            }
        }

        if (maxPrice >= 100_000 && maxPrice < 1_000_000)
        {
            strMax = toStringMaxPrice.Split(",")[0] + "k";
        }
        else if (maxPrice >= 1_000_000 && maxPrice < 10_000_000)
        {
            strMax = toStringMaxPrice.Split(",")[0];
            int maxPriceParse = Convert.ToInt32(toStringMaxPrice.Split(",")[1]);
            if (maxPriceParse >= 100)
            {
                strMax = strMax + "." + maxPriceParse.ToString().Substring(0, maxPriceParse.ToString().Length - 2);
            }
        }
        else if (maxPrice >= 10_000_000 && maxPrice < 100_000_000)
        {
            strMax = toStringMaxPrice.Split(",")[0];
            int maxPriceParse = Convert.ToInt32(toStringMaxPrice.Split(",")[1]);
            if (maxPriceParse >= 100)
            {
                strMax = strMax + "." + maxPriceParse.ToString().Substring(0, maxPriceParse.ToString().Length - 2);
            }
        }

        if (minPrice == maxPrice)
        {
            if (maxPrice < 1_000_000)
            {
                return $"Chỉ từ {strMax} /tháng"; //
            }

            return $"Chỉ từ {strMax} {baseString}";
        }

        if (minPrice > maxPrice)
        {
            return $"{strMax} - {strMin} {baseString}";
        }

        return $"{strMin} - {strMax} {baseString}";
    }

    public static string ToShortenMoneyOneParam(int? price)
    {
        const string baseStringK = "/tháng";
        const string baseStringT = "triệu/tháng";

        if (price == null)
        {
            return "Phòng chưa có giá";
        }

        if (price <= 0)
        {
            return "Phòng chưa có giá";
        }

        string toStringPrice = string.Format("{0:#,##0.##}", price);

        string strPrice = "";

        if (price >= 100_000 && price < 1_000_000)
        {
            strPrice = toStringPrice.Split(",")[0] + "k";
        }
        else if (price >= 1_000_000 && price < 10_000_000)
        {
            strPrice = toStringPrice.Split(",")[0];
            int priceParse = Convert.ToInt32(toStringPrice.Split(",")[1]);
            if (priceParse >= 100)
            {
                strPrice = strPrice + "." + priceParse.ToString().Substring(0, priceParse.ToString().Length - 2);
            }
        }
        else if (price >= 10_000_000 && price < 100_000_000)
        {
            strPrice = toStringPrice.Split(",")[0];
            int priceParse = Convert.ToInt32(toStringPrice.Split(",")[1]);
            if (priceParse >= 100)
            {
                strPrice = strPrice + "." + priceParse.ToString().Substring(0, priceParse.ToString().Length - 2);
            }
        }

        if (price < 1_000_000)
        {
            return $"{strPrice} {baseStringK}";
        }

        return $"{strPrice} {baseStringT}";
    }

    public static string NonUnicode(this string text)
    {
        string[] arr1 =
        {
            "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "đ", "é", "è", "ẻ",
            "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ", "í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ",
            "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý",
            "ỳ", "ỷ", "ỹ", "ỵ"
        };
        string[] arr2 =
        {
            "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "d", "e", "e", "e",
            "e", "e", "e", "e", "e", "e", "e", "e", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o", "o", "o",
            "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "y",
            "y", "y", "y", "y"
        };
        for (int i = 0; i < arr1.Length; i++)
        {
            text = text.Replace(arr1[i], arr2[i]);
            text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
        }

        return text;
    }

    public static string RegexReplace(this string input, string regx, string replacement)
    {
        return Regex.Replace(input, regx, replacement);
    }

    public static string ReplaceWhitespace(this string input, string replacement = "")
    {
        return input.RegexReplace(@"\s+", replacement);
    }

    public static string KeepFirstChars(this string input, int strlen)
    {
        return input.Substring(0, Math.Min(strlen, input.Length));
    }

    public static string GenerateUsername(string name)
    {
        string guid = Guid.NewGuid().ToString().Split('-')[1];
        string username = name
            .ToLower()
            .NonUnicode()
            .RegexReplace(@"[^0-9a-zA-Z]+", string.Empty)
            .KeepFirstChars(7)
            .ReplaceWhitespace();

        return $"{username}_{guid}";
    }

    public static bool HasSpecialChar(this string input)
    {
        const string pattern = "[^A-Za-z0-9 ]";

        var containsSpecialChar = Regex.IsMatch(input, pattern);

        return containsSpecialChar;
    }

    public static bool IsUrl(this string input)
    {
        bool result = Uri.TryCreate(input, UriKind.Absolute, out Uri? uriResult)
                      && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return result;
    }

    public static bool IsNullOrEmptyOrWhiteSpace(this string? input)
    {
        return string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input);
    }

    public static string ShortNumberLikeCommentShare(int total)
    {
        const string baseString = "k";
        string toString = $"{total:#,##0.##}";
        string str = "";

        if (total < 1000)
        {
            return total.ToString();
        }

        string s1 = toString.Split(",")[0];
        string s2 = toString.Split(",")[1];

        if (Int32.Parse(s2) > 99)
        {
            str = s1 + "." + s2.Substring(0, s2.Length - 2) + baseString;
        }
        else
        {
            str = s1 + baseString;
        }

        return str;
    }

    public static bool IsValidDate(string date) // Simple date dd/mm/yyyy
    {
        return Regex.IsMatch(date, @"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$");
    }

    private static readonly string[] PhonePrefixes =
    {
        "086", "096", "097", "098", "032", "033",
        "034", "035", "036", "037", "038", "039",
        "089", "090", "093", "070", "079", "077", "076", "078",
        "088", "091", "094", "083", "084", "085", "081", "082",
        "092", "056", "058",
        "099", "059"
    };

    public static bool IsValidDomain(string value, string domain)
    {
        var pattern = @"^(https?:\/\/)?(www\.)?" + domain + @"\.com\/?";

        return Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase);
    }

    public static bool IsPhoneNumberVN(this string phoneNumber)
    {
        // Check if the input contains any non-digit characters
        if (!Regex.IsMatch(phoneNumber, @"^[0-9]+$"))
        {
            return false;
        }
        
        // Remove any non-digit characters
        var cleanedNumber = Regex.Replace(phoneNumber, @"\D", "");

        // Check the length of the cleaned number
        if (cleanedNumber.Length != 10 && cleanedNumber.Length != 11)
        {
            return false;
        }

        return PhonePrefixes.Contains(cleanedNumber.Substring(0, 3));

        // All checks passed, the phone number is valid
    }

    public static bool IsEmail(this string input) // Simple date dd/mm/yyyy
    {
        return Regex.IsMatch(input, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
    }

    public static string GenerateFileName(IFormFile file) // Simple date dd/mm/yyyy
    {
        return UniqueString() + "_" + file.FileName;
    }

    public static List<string> ExtractEmails(this string input)
    {
        var outputEmails = new List<string>();
        const string matchEmailPattern =
            @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
        var rx = new Regex(
            matchEmailPattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);
        // Find matches.
        var matches = rx.Matches(input);
        // Report the number of matches found.
        int noOfMatches = matches.Count;
        // Report on each match.
        foreach (Match match in matches)
        {
            outputEmails.Add(match.Value.ToString());
        }

        return outputEmails;
    }

    public static string ToQueryString<T>(this T model)
    {
        var sb = new StringBuilder();
        var properties = model!.GetType().GetProperties();
        foreach (var property in properties)
        {
            var value = property.GetValue(model);
            if (value != null && !IsNullOrEmptyOrWhiteSpace(value.ToString()))
            {
                var key = property.Name;
                var displayAttr = property.GetCustomAttribute<DescriptionAttribute>();
                if (displayAttr != null)
                {
                    key = displayAttr.Description;
                }

                sb.Append($"&{key}={value}");
            }
        }

        return sb.ToString();
    }

    public static bool CheckCronExpression(this string cronExpression)
    {
        var listDenied = new List<string>() { "* * * * * *", "* * * * *", "*/2 * * * *", "1-59/2 * * * *" };

        for (var i = 1; i <= 60; i++)
        {
            listDenied.Add($"*/{i} * * * *");
        }

        return listDenied.Contains(cronExpression);
    }

    public static bool IsValidHttpMethod(this string input)
    {
        return HttpMethods.ToList().Contains(input.ToUpper());
    }

    public static string MaskEmails(this string input)
    {
        var pattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
        return Regex.Replace(input, pattern,
            match => "****" + match.Value.Substring(match.Value.IndexOf("@", StringComparison.Ordinal)));
    }

    public static TimeZoneInfo TimeZoneVietnamese()
    {
        const string displayName = "(GMT+7) Asia/Viet Nam";
        const string standardName = "VN";
        TimeSpan offset = new(07, 00, 00);
        TimeZoneInfo vnTimeZone = TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName);

        return vnTimeZone;
    }

    public static string ConvertIFormFileToBase64(this IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();
        var base64String = Convert.ToBase64String(fileBytes);
        return base64String;
    }

    public static string ConvertWildcardCharacters(this string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return "";
        }

        text = text.Trim();
        text = text.Replace("[", "[[]");
        text = text.Replace("%", "[%]");
        text = text.Replace("_", "[_]");

        return text;
    }

    public static string FormatDateTime(this DateTime input)
    {
        return input.ToString("dd/MM/yyyy hh:mm tt");
    }

    #region readonly variable

    private static readonly string[] HttpMethods =
    {
        "GET", "POST", "HEAD", "PUT", "PATCH", "DELETE", "CONNECT", "OPTIONS", "TRACE"
    };

    private static readonly string[] SpecialCharacters =
    {
        "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "-", "+", "=", "{", "}", "\\", ":", ";",
        "\"", "'", ",", "<", ".", ">", "/", "?"
    };

    private static readonly string[] VietnameseSigns =
    {
        "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
    };

    private static readonly string SpecialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
    private static readonly string[] Suffixes = { "Bytes", "KB", "MB", "GB", "TB", "PB" };
    private static readonly string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static readonly string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
    private static readonly string NumberCharacters = "0123456789";

    #endregion readonly variable
}