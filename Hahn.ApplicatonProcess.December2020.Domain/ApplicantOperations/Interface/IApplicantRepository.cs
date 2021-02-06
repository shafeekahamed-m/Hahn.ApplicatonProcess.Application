using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface
{
    ///<Summary>
    /// IApplicantRepository
    ///</Summary>
    public interface IApplicantRepository
    {
        ///<Summary>
        /// GetApplicant method
        ///</Summary>
        public List<Applicant> GetApplicant(int ID);
        ///<Summary>
        /// GetAllApplicant method
        ///</Summary>
        public List<Applicant> GetAllApplicant();
        ///<Summary>
        /// ApplicantValidator method
        ///</Summary>
        public int SaveApplicant(Applicant objApplicant);
        ///<Summary>
        /// UpdateApplicant method
        ///</Summary>
        public int UpdateApplicant(Applicant objApplicant);
        ///<Summary>
        /// DeleteApplicant method
        ///</Summary>
        public int DeleteApplicant(int ID);
    }
}
