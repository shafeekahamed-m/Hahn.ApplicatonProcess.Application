﻿using FluentValidation;
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
        public string LastUpdated { get; set; }
    }
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator()
        {
            RuleSet("IDNotNull", () =>
            {
                //RuleFor(x => x.Id).Must(CheckId).WithMessage("id must greater than 0");
                //RuleFor(x => x.Name).NotNull().When(x => !x.Id.HasValue).WithMessage("name could not be null");
                RuleFor(x => x.ID).NotNull().GreaterThan(0);
            });
            RuleSet("save", () =>
            {
                //RuleFor(x => x.Id).Must(CheckId).WithMessage("id must greater than 0");
                //RuleFor(x => x.Name).NotNull().When(x => !x.Id.HasValue).WithMessage("name could not be null");
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
                            //response.EnsureSuccessStatusCode();
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
                //RuleFor(x => x.Hired).Must(CheckHired).WithMessage("'Hired' can only be null or either a boolen value(Eg.true or false)");
            });
            //RuleSet("update", () =>
            //{
            //    //RuleFor(x => x.Id).Must(CheckId).WithMessage("id must greater than 0");
            //    //RuleFor(x => x.Name).NotNull().When(x => !x.Id.HasValue).WithMessage("name could not be null");
            //    RuleFor(x => x.ID).NotNull().GreaterThan(0);
            //    RuleFor(x => x.Name).Length(5);
            //    RuleFor(x => x.FamilyName).Length(5);
            //    RuleFor(x => x.Address).Length(10);
            //    RuleFor(x => x.CountryOfOrigin).Must(IsValidCountry).WithMessage("'Country Of Origin' must be a valid Country");
            //    RuleFor(x => x.EMailAddress).Must(IsValidEmail).EmailAddress();
            //    RuleFor(x => x.Age).InclusiveBetween(18, 60);
            //});
        }
        private bool CheckHired(bool id)
        {
            return false;//!id.HasValue || id.Value > 0;
        }

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
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
        //public bool IsValidCountry(string? Country)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var response = client.GetAsync("https://restcountries.eu//rest/v2/name/" + Country + "?fullText=true").GetAwaiter().GetResult();
        //        //if (response.IsSuccessStatusCode)
        //        //{
        //        //    var responseContent = response.Content;
        //        //    var check= responseContent.ReadAsStringAsync().GetAwaiter().GetResult();
        //        //}
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        JsonConvert.DeserializeObject<Countries>(response.Content);
        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        var objText = reader.ReadToEnd();
        //        MyObject myojb = (MyObject)js.Deserialize(objText, typeof(MyObject));
        //        return response.IsSuccessStatusCode;
        //    }
        //    //using (var client = new WebClient())
        //    //{
        //    //    string response = client.DownloadString();
        //    //    if (!string.IsNullOrEmpty(response))
        //    //    {
        //    //        var item = response;
        //    //    }
        //    //}
        //    //using (var client = new HttpClient())
        //    //{
        //    //    client.BaseAddress = new Uri("https://restcountries.eu//");
        //    //    client.DefaultRequestHeaders.Accept.Clear();
        //    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //    var response =  client.GetAsync("rest/v2/name/"+Country+ "?fullText=true");
        //    //    List<string> li;
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        Countries obj = new Countries();

        //    //        var details = response.Content.ReadAsAsync<IEnumerable<Countries>>().Result;
        //    //        return false;


        //    //    }
        //    //    else
        //    //    {
        //    //        return false;

        //    //    }
        //    //}
        //}
    }
}
