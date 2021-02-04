using Hahn.ApplicatonProcess.December2020.Domain;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Interface;
using Hahn.ApplicatonProcess.December2020.Domain.ApplicantOperations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.December2020.Data
{
    public class InMemoryApplicantRepository : IApplicantRepository
    {
        private readonly ApiContext _context;
        public InMemoryApplicantRepository(ApiContext context)
        {
            _context = context;
        }
        public List<Applicant> GetApplicant(int ID)
        {
            return _context.Applicants.Where(x => x.ID == ID).ToList();
        }
        public List<Applicant> GetAllApplicant()
        {
            return _context.Applicants.OrderByDescending(x=>Convert.ToDateTime(x.LastUpdated)).ToList();
        }
        public int SaveApplicant(Applicant objApplicant)
        {
            int nextId = (_context.Applicants.Count()==0)?1:_context.Applicants.Max(x => x.ID)+1;
            _context.Applicants.Add(new Applicant
            {
                ID = nextId,
                Name = objApplicant.Name,
                FamilyName = objApplicant.FamilyName,
                Address = objApplicant.Address,
                CountryOfOrigin = objApplicant.CountryOfOrigin,
                EMailAddress = objApplicant.EMailAddress,
                Age = objApplicant.Age,
                Hired = objApplicant.Hired,
                LastUpdated= DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt")
            });
            _context.SaveChanges();
            return nextId;
        }
        public int UpdateApplicant(Applicant objApplicant)
        {
            int returnId = 1;
            if (_context.Applicants.Any(x => x.ID == objApplicant.ID))
            {
                foreach (var item in _context.Applicants.Where(x => x.ID == objApplicant.ID))
                {
                    item.Name = objApplicant.Name;
                    item.FamilyName = objApplicant.FamilyName;
                    item.Address = objApplicant.Address;
                    item.CountryOfOrigin = objApplicant.CountryOfOrigin;
                    item.EMailAddress = objApplicant.EMailAddress;
                    item.Age = objApplicant.Age;
                    item.Hired = objApplicant.Hired;
                    item.LastUpdated = DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt");
                }
                _context.SaveChanges();
            }
            else
            {
                returnId = -1;
            }
            return returnId;
        }
        public int DeleteApplicant(int ID)
        {
            int returnId = 1;
            if(_context.Applicants.Any(x => x.ID == ID))
            {
                foreach (var item in _context.Applicants.Where(x => x.ID == ID))
                {
                    _context.Applicants.Remove(item);
                }
                _context.SaveChanges();
            }
            else
            {
                returnId = -1;
            }
            return returnId;
        }
        
    }
}
