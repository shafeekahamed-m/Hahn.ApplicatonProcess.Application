using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models
{
    /// <summary>
    /// Applicant class
    /// </summary>
    public class Applicant
    {
        /// <summary>
        /// Id not needed during post
        /// </summary>
        /// <example>3</example>
        public int ID { get; set; }
        /// <summary>
        /// Any Valid Name
        /// </summary>
        /// <example>Luke Skywalker</example>
        public string Name { get; set; }
        /// <summary>
        /// Any Valid Name
        /// </summary>
        /// <example>George Skywalker</example>
        public string FamilyName { get; set; }
        /// <summary>
        /// Any Valid Name
        /// </summary>
        /// <example>133,2nd st,Outer Nebula</example>
        public string Address { get; set; }
        /// <summary>
        /// Any Valid Name
        /// </summary>
        /// <example>Germany</example>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Any Valid EMailAddress
        /// </summary>
        /// <example>shafeekahamed21@gmail.com</example>
        public string EMailAddress { get; set; }
        /// <summary>
        /// Any Valid Age
        /// </summary>
        /// <example>26</example>
        public int Age { get; set; }
        /// <summary>
        /// Any Valid Boolean
        /// </summary>
        /// <example>true</example>
        public bool Hired { get; set; }
        ///<Summary>
        /// LastUpdated
        ///</Summary>
        public string LastUpdated { get; set; }
    }
    ///<Summary>
    /// ApplicantValidator
    ///</Summary>
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        ///<Summary>
        /// ApplicantValidator method
        ///</Summary>
        public ApplicantValidator()
        {
            RuleSet("IDNotNull", () =>
            {
                RuleFor(x => x.ID).NotNull().GreaterThan(0);
            });
            RuleSet("save", () =>
            {
                RuleFor(x => x.Name).NotNull().MinimumLength(5);
                RuleFor(x => x.FamilyName).NotNull().MinimumLength(5);
                RuleFor(x => x.Address).NotNull().MinimumLength(10);
                RuleFor(x => x.CountryOfOrigin).NotNull().MustAsync(async (Country, cancellation) => {
                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage(HttpMethod.Get, "https://restcountries.eu//rest/v2/name/" + Country + "?fullText=true"))
                    using (var response = await client.SendAsync(request))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var check = JsonConvert.DeserializeObject<List<CheckCountry>>(content);
                            foreach (var item in check)
                            {
                                if(item.name.ToUpper()== Country.ToUpper())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }).WithMessage("'Country Of Origin' must be a valid Country");
                RuleFor(x => x.EMailAddress).NotNull().Must(IsValidEmail).EmailAddress();
                RuleFor(x => x.Age).NotNull().InclusiveBetween(20, 60);
            });
        }
        private bool CheckHired(bool id)
        {
            return false;
        }
        #nullable enable
        private static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private async Task<bool> IsValidCountry(string? Country)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://restcountries.eu//rest/v2/name/" + Country + "?fullText=true"))
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var check= JsonConvert.DeserializeObject<List<Countries>>(content);
            }
            return true;
        }
    }
}
