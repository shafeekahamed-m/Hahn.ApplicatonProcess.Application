using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface
{
    ///<Summary>
    /// IApplicantService
    ///</Summary>
    public interface IApplicantService
    {
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public List<Applicant> GetApplicant(int ID);
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public List<Applicant> GetAllApplicant();
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public int SaveApplicant(Applicant objApplicant);
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public int UpdateApplicant(Applicant objApplicant);
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public int DeleteApplicant(int ID);
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public Message ApplicantValidate(Applicant objApplicant, bool IsUpdate);
    }
}
