using FluentValidation;
using FluentValidation.Results;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations
{
    public class ApplicantService:IApplicantService
    {
        private readonly IApplicantRepository _applicantRepository;
        private readonly AbstractValidator<Applicant> _validator;
        public ApplicantService(IApplicantRepository applicantRepository, AbstractValidator<Applicant> validator)
        {
            _applicantRepository = applicantRepository;
            _validator = validator;
        }
        public Message ApplicantValidate(Applicant objApplicant,bool IsUpdate)
        {
            string[] rules = { "save", "IDNotNull" };
            ValidationResult res = null;
            if (IsUpdate)
            {
                res = _validator.Validate(objApplicant, options => options.IncludeRuleSets(rules));
            }
            else
            {
                res = _validator.Validate(objApplicant, options => options.IncludeRuleSets("save"));
            }
            
            Message objMessage = null;
            if (!res.IsValid)
            {
                objMessage = new Message();
                objMessage.Code = -1;
                //objMessage.Messages =(List<string>) res.Errors;   
                foreach(var item in res.Errors)
                {
                    objMessage.Messages.Add(Convert.ToString(item));
                }
                return objMessage;
            }
            else
            {
                objMessage = new Message();
                objMessage.Code = 1;
                return objMessage;
            }
        }
        public List<Applicant> GetApplicant(int ID)
        {
            return _applicantRepository.GetApplicant(ID);
        }
        public List<Applicant> GetAllApplicant()
        {
            return _applicantRepository.GetAllApplicant();
        }
        public int SaveApplicant(Applicant objApplicant)
        {
            return _applicantRepository.SaveApplicant(objApplicant);
        }
        public int DeleteApplicant(int ID)
        {
            return _applicantRepository.DeleteApplicant(ID);
        }
        public int UpdateApplicant(Applicant objApplicant)
        {
            return _applicantRepository.UpdateApplicant(objApplicant);
        }
    }
}
