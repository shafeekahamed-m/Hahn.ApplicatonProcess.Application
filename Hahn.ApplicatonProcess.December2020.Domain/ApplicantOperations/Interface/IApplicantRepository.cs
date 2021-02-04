using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface
{
    public interface IApplicantRepository
    {
        public List<Applicant> GetApplicant(int ID);
        public List<Applicant> GetAllApplicant();
        public int SaveApplicant(Applicant objApplicant);
        public int UpdateApplicant(Applicant objApplicant);
        public int DeleteApplicant(int ID);
    }
}
